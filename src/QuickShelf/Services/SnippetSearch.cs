using QuickShelf.Models;

namespace QuickShelf.Services;

public static class SnippetSearch
{
    public static bool Matches(Snippet snippet, string? query)
    {
        if (string.IsNullOrWhiteSpace(query)) return true;
        var term = query.Trim();
        return snippet.Title.Contains(term, StringComparison.CurrentCultureIgnoreCase)
            || snippet.Content.Contains(term, StringComparison.CurrentCultureIgnoreCase);
    }
}
