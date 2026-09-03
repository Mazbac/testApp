using QuickShelf.Models;
using QuickShelf.Services;

namespace QuickShelf.Tests;

[TestClass]
public sealed class StateStoreTests
{
    private string _directory = null!;

    [TestInitialize]
    public void Setup()
    {
        _directory = Path.Combine(Path.GetTempPath(), "QuickShelf.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_directory);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(_directory)) Directory.Delete(_directory, true);
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task SaveThenLoad_PreservesSnippetAndSettings()
    {
        var store = new StateStore(_directory);
        var state = new AppState
        {
            Settings = new AppSettings { Theme = ThemePreference.Dark },
            Snippets = [new Snippet { Title = "Alpha", Content = "Beta", IsFavorite = true }]
        };

        await store.SaveAsync(state);
        var loaded = await store.LoadAsync();
        Assert.AreEqual(ThemePreference.Dark, loaded.State.Settings.Theme);
        Assert.HasCount(1, loaded.State.Snippets);
        Assert.AreEqual("Alpha", loaded.State.Snippets[0].Title);
        Assert.AreEqual("Beta", loaded.State.Snippets[0].Content);
        Assert.IsTrue(loaded.State.Snippets[0].IsFavorite);
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task CorruptPrimary_RecoversFromLastGoodBackup()
    {
        var store = new StateStore(_directory);
        await store.SaveAsync(new AppState { Snippets = [new Snippet { Title = "First" }] });
        await store.SaveAsync(new AppState { Snippets = [new Snippet { Title = "Second" }] });
        await File.WriteAllTextAsync(store.StatePath, "{ definitely not json");

        var loaded = await store.LoadAsync();

        Assert.AreEqual("First", loaded.State.Snippets.Single().Title);
        Assert.IsFalse(string.IsNullOrWhiteSpace(loaded.RecoveryMessage));
        Assert.IsTrue(Directory.EnumerateFiles(_directory, "quickshelf.corrupt-*.json").Any());
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task InvalidImport_IsRejectedWithoutChangingStoredState()
    {
        var store = new StateStore(_directory);
        await store.SaveAsync(new AppState { Snippets = [new Snippet { Title = "Keep me" }] });
        var importPath = Path.Combine(_directory, "bad.json");
        await File.WriteAllTextAsync(importPath, "{\"schemaVersion\":99,\"snippets\":[],\"settings\":{\"theme\":\"System\"}}");

        await Assert.ThrowsExactlyAsync<InvalidDataException>(() => store.ReadImportAsync(importPath));
        var loaded = await store.LoadAsync();
        Assert.AreEqual("Keep me", loaded.State.Snippets.Single().Title);
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task Reset_ReplacesStateWithEmptyShelf()
    {
        var store = new StateStore(_directory);
        await store.SaveAsync(new AppState { Snippets = [new Snippet { Title = "Delete me" }] });
        await store.ResetAsync();

        var loaded = await store.LoadAsync();
        Assert.IsEmpty(loaded.State.Snippets);
        Assert.AreEqual(ThemePreference.System, loaded.State.Settings.Theme);
    }
}
