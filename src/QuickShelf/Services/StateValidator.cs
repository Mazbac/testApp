using System.IO;
using QuickShelf.Models;

namespace QuickShelf.Services;

public static class StateValidator
{
    public const long MaxImportBytes = 5 * 1024 * 1024;
    public const int MaxSnippets = 10_000;
    public const int MaxTitleLength = 200;
    public const int MaxContentLength = 100_000;

    public static void Validate(AppState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        if (state.SchemaVersion != AppState.CurrentSchemaVersion)
            throw new InvalidDataException($"Unsupported QuickShelf schema version {state.SchemaVersion}.");
        if (state.Snippets is null || state.Settings is null)
            throw new InvalidDataException("QuickShelf data is incomplete.");
        if (state.Snippets.Count > MaxSnippets)
            throw new InvalidDataException($"QuickShelf data contains more than {MaxSnippets:N0} snippets.");

        var ids = new HashSet<Guid>();
        foreach (var snippet in state.Snippets)
        {
            if (snippet is null || snippet.Id == Guid.Empty || !ids.Add(snippet.Id))
                throw new InvalidDataException("QuickShelf data contains an invalid or duplicate snippet identifier.");
            if (snippet.Title is null || snippet.Title.Length > MaxTitleLength)
                throw new InvalidDataException($"A snippet title exceeds {MaxTitleLength} characters.");
            if (snippet.Content is null || snippet.Content.Length > MaxContentLength)
                throw new InvalidDataException($"A snippet exceeds {MaxContentLength:N0} characters.");
            if (snippet.CreatedAtUtc == default || snippet.UpdatedAtUtc == default)
                throw new InvalidDataException("A snippet has an invalid timestamp.");
        }
    }
}
