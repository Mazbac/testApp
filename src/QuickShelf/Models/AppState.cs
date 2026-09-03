namespace QuickShelf.Models;

public sealed class AppState
{
    public const int CurrentSchemaVersion = 1;

    public int SchemaVersion { get; set; } = CurrentSchemaVersion;
    public List<Snippet> Snippets { get; set; } = [];
    public AppSettings Settings { get; set; } = new();

    public AppState Clone() => new()
    {
        SchemaVersion = SchemaVersion,
        Snippets = Snippets.Select(snippet => snippet.Clone()).ToList(),
        Settings = Settings.Clone()
    };
}

public sealed record LoadResult(AppState State, string? RecoveryMessage = null);
