using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using QuickShelf.Models;

namespace QuickShelf.Services;

public sealed class StateStore
{
    private readonly JsonSerializerOptions _json = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public StateStore(string? directory = null)
    {
        var testOverride = Environment.GetEnvironmentVariable("QUICKSHELF_DATA_DIR");
        AppDirectory = directory ?? (!string.IsNullOrWhiteSpace(testOverride)
            ? Path.GetFullPath(testOverride)
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QuickShelf"));
        StatePath = Path.Combine(AppDirectory, "quickshelf.json");
        BackupPath = Path.Combine(AppDirectory, "quickshelf.json.bak");
    }

    public string AppDirectory { get; }
    public string StatePath { get; }
    public string BackupPath { get; }

    public async Task<LoadResult> LoadAsync(CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(AppDirectory);
        if (!File.Exists(StatePath)) return new LoadResult(new AppState());

        try
        {
            return new LoadResult(await ReadAndValidateAsync(StatePath, cancellationToken));
        }
        catch (Exception exception) when (IsRecoverableReadFailure(exception))
        {
            var quarantined = Quarantine(StatePath);
            if (File.Exists(BackupPath))
            {
                try
                {
                    var recovered = await ReadAndValidateAsync(BackupPath, cancellationToken);
                    await SaveAsync(recovered, cancellationToken);
                    return new LoadResult(recovered, "QuickShelf recovered your snippets from the last good backup after detecting damaged local data.");
                }
                catch (Exception backupException) when (IsRecoverableReadFailure(backupException))
                {
                    // Fall through to a clean state; both files remain available for diagnostics except the quarantined primary.
                }
            }

            return new LoadResult(new AppState(), $"QuickShelf could not read its local data. The damaged file was preserved as {Path.GetFileName(quarantined)} and a clean shelf was opened.");
        }
    }

    public async Task SaveAsync(AppState state, CancellationToken cancellationToken = default)
    {
        StateValidator.Validate(state);
        Directory.CreateDirectory(AppDirectory);
        var tempPath = Path.Combine(AppDirectory, $"quickshelf.{Guid.NewGuid():N}.tmp");

        try
        {
            await WriteStateAsync(state, tempPath, cancellationToken);
            if (File.Exists(StatePath))
                File.Replace(tempPath, StatePath, BackupPath, true);
            else
                File.Move(tempPath, StatePath);
        }
        finally
        {
            if (File.Exists(tempPath)) File.Delete(tempPath);
        }
    }

    public async Task<AppState> ReadImportAsync(string path, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return await ReadAndValidateAsync(path, cancellationToken);
    }

    public async Task ExportAsync(AppState state, string path, CancellationToken cancellationToken = default)
    {
        StateValidator.Validate(state);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        var fullPath = Path.GetFullPath(path);
        var directory = Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException("Export folder is invalid.");
        Directory.CreateDirectory(directory);
        var tempPath = Path.Combine(directory, $".{Path.GetFileName(fullPath)}.{Guid.NewGuid():N}.tmp");
        try
        {
            await WriteStateAsync(state, tempPath, cancellationToken);
            File.Move(tempPath, fullPath, true);
        }
        finally
        {
            if (File.Exists(tempPath)) File.Delete(tempPath);
        }
    }

    public async Task ResetAsync(CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(AppDirectory);
        foreach (var file in Directory.EnumerateFiles(AppDirectory, "quickshelf*.json*"))
            File.Delete(file);
        await SaveAsync(new AppState(), cancellationToken);
    }

    private async Task<AppState> ReadAndValidateAsync(string path, CancellationToken cancellationToken)
    {
        var info = new FileInfo(path);
        if (!info.Exists) throw new FileNotFoundException("QuickShelf data file was not found.", path);
        if (info.Length > StateValidator.MaxImportBytes)
            throw new InvalidDataException($"QuickShelf data is larger than the {StateValidator.MaxImportBytes / 1024 / 1024} MB safety limit.");

        await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 64 * 1024, true);
        var state = await JsonSerializer.DeserializeAsync<AppState>(stream, _json, cancellationToken)
            ?? throw new InvalidDataException("QuickShelf data is empty.");
        StateValidator.Validate(state);
        return state;
    }

    private async Task WriteStateAsync(AppState state, string path, CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 64 * 1024, true);
        await JsonSerializer.SerializeAsync(stream, state, _json, cancellationToken);
        await stream.FlushAsync(cancellationToken);
        stream.Flush(true);
    }

    private string Quarantine(string path)
    {
        var quarantine = Path.Combine(AppDirectory, $"quickshelf.corrupt-{DateTime.UtcNow:yyyyMMdd-HHmmssfff}.json");
        File.Move(path, quarantine, true);
        return quarantine;
    }

    private static bool IsRecoverableReadFailure(Exception exception) => exception is
        IOException or UnauthorizedAccessException or JsonException or InvalidDataException;
}
