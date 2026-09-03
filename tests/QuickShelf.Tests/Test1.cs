using QuickShelf.Models;
using QuickShelf.Services;

namespace QuickShelf.Tests;

[TestClass]
public sealed class SnippetRulesTests
{
    [TestMethod]
    [TestCategory("Unit")]
    public void Search_MatchesTitleAndContent_CaseInsensitively()
    {
        var snippet = new Snippet { Title = "Release checklist", Content = "Remember the installer smoke test" };
        Assert.IsTrue(SnippetSearch.Matches(snippet, "RELEASE"));
        Assert.IsTrue(SnippetSearch.Matches(snippet, "Installer"));
        Assert.IsFalse(SnippetSearch.Matches(snippet, "banana"));
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void Search_EmptyQuery_MatchesEverything()
    {
        Assert.IsTrue(SnippetSearch.Matches(new Snippet(), "   "));
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void Validator_RejectsDuplicateSnippetIds()
    {
        var id = Guid.NewGuid();
        var state = new AppState { Snippets = [new Snippet { Id = id }, new Snippet { Id = id }] };
        Assert.ThrowsExactly<InvalidDataException>(() => StateValidator.Validate(state));
    }

    [TestMethod]
    [TestCategory("Unit")]
    public void Validator_RejectsFutureSchema()
    {
        var state = new AppState { SchemaVersion = AppState.CurrentSchemaVersion + 1 };
        Assert.ThrowsExactly<InvalidDataException>(() => StateValidator.Validate(state));
    }
}
