using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using QuickShelf.Models;
using QuickShelf.Services;

namespace QuickShelf;

public partial class MainWindow : Window
{
    private readonly StateStore _store = new();
    private readonly ObservableCollection<Snippet> _snippets = [];
    private readonly DispatcherTimer _saveTimer = new() { Interval = TimeSpan.FromMilliseconds(550) };
    private ICollectionView? _snippetView;
    private AppSettings _settings = new();
    private Snippet? _selected;
    private bool _dirty;
    private bool _loading;
    private bool _allowClose;

    public MainWindow()
    {
        InitializeComponent();
        _saveTimer.Tick += SaveTimer_Tick;
        ThemeService.Apply(ThemePreference.System);
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _loading = true;
        try
        {
            var result = await _store.LoadAsync();
            ApplyState(result.State);
            if (!string.IsNullOrWhiteSpace(result.RecoveryMessage)) ShowNotice(result.RecoveryMessage);
        }
        catch (Exception exception)
        {
            ApplyState(new AppState());
            ShowNotice($"QuickShelf could not load local data: {exception.Message}");
        }
        finally
        {
            _loading = false;
            UpdateSelectionSurface();
        }
    }

    private void ApplyState(AppState state)
    {
        foreach (var existing in _snippets) existing.PropertyChanged -= Snippet_PropertyChanged;
        _snippets.Clear();
        _settings = state.Settings.Clone();
        ThemeService.Apply(_settings.Theme);

        foreach (var snippet in state.Snippets)
        {
            snippet.PropertyChanged += Snippet_PropertyChanged;
            _snippets.Add(snippet);
        }

        _snippetView ??= CollectionViewSource.GetDefaultView(_snippets);
        _snippetView.Filter = item => item is Snippet snippet && SnippetSearch.Matches(snippet, SearchBox.Text);
        _snippetView.SortDescriptions.Clear();
        _snippetView.SortDescriptions.Add(new SortDescription(nameof(Snippet.IsFavorite), ListSortDirection.Descending));
        _snippetView.SortDescriptions.Add(new SortDescription(nameof(Snippet.UpdatedAtUtc), ListSortDirection.Descending));
        SnippetList.ItemsSource = _snippetView;
        _snippetView.Refresh();
        UpdateCount();

        if (_snippets.Count > 0)
        {
            _snippetView.MoveCurrentToFirst();
            SnippetList.SelectedItem = _snippetView.CurrentItem;
        }
        else
        {
            SnippetList.SelectedItem = null;
        }
    }

    private void Snippet_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_loading || sender is not Snippet snippet) return;
        if (e.PropertyName != nameof(Snippet.UpdatedAtUtc)) snippet.UpdatedAtUtc = DateTimeOffset.UtcNow;
        _dirty = true;
        SaveStatusText.Text = "Saving...";
        UpdatedText.Text = $"Last edited {snippet.UpdatedAtUtc.ToLocalTime():g}";
        _snippetView?.Refresh();
        RestartSaveTimer();
    }

    private void RestartSaveTimer()
    {
        _saveTimer.Stop();
        _saveTimer.Start();
    }

    private async void SaveTimer_Tick(object? sender, EventArgs e)
    {
        _saveTimer.Stop();
        await SaveNowAsync();
    }

    private AppState CreateSnapshot() => new()
    {
        Snippets = _snippets.Select(snippet => snippet.Clone()).ToList(),
        Settings = _settings.Clone()
    };

    private async Task<bool> SaveNowAsync()
    {
        _saveTimer.Stop();
        if (!_dirty) return true;
        try
        {
            await _store.SaveAsync(CreateSnapshot());
            _dirty = false;
            SaveStatusText.Text = $"Saved {DateTime.Now:t}";
            return true;
        }
        catch (Exception exception)
        {
            SaveStatusText.Text = "Save failed";
            ShowNotice($"QuickShelf could not save your latest changes. Your current window is still open. {exception.Message}");
            return false;
        }
    }

    private void MarkSettingsDirty()
    {
        if (_loading) return;
        _dirty = true;
        SaveStatusText.Text = "Saving...";
        RestartSaveTimer();
    }

    private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        _snippetView?.Refresh();
    }

    private void SnippetList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        _selected = SnippetList.SelectedItem as Snippet;
        UpdateSelectionSurface();
    }

    private void UpdateSelectionSurface()
    {
        var hasSelection = _selected is not null;
        EditorPanel.Visibility = hasSelection ? Visibility.Visible : Visibility.Collapsed;
        EmptyState.Visibility = hasSelection ? Visibility.Collapsed : Visibility.Visible;
        EditorPanel.DataContext = _selected;

        if (_selected is not null)
        {
            FavoriteButton.Content = _selected.IsFavorite ? "Favorited" : "Favorite";
            UpdatedText.Text = $"Last edited {_selected.UpdatedAtUtc.ToLocalTime():g}";
        }
        UpdateCount();
    }

    private void UpdateCount()
    {
        SnippetCountText.Text = _snippets.Count == 1 ? "1 snippet" : $"{_snippets.Count} snippets";
    }

    private void NewSnippet_Click(object sender, RoutedEventArgs e)
    {
        var snippet = new Snippet();
        snippet.PropertyChanged += Snippet_PropertyChanged;
        _snippets.Add(snippet);
        _dirty = true;
        _snippetView?.Refresh();
        SnippetList.SelectedItem = snippet;
        SnippetList.ScrollIntoView(snippet);
        UpdateCount();
        RestartSaveTimer();
        TitleBox.Focus();
        TitleBox.SelectAll();
    }

    private void FavoriteButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selected is null) return;
        _selected.IsFavorite = !_selected.IsFavorite;
        FavoriteButton.Content = _selected.IsFavorite ? "Favorited" : "Favorite";
    }

    private void DeleteSnippet_Click(object sender, RoutedEventArgs e)
    {
        if (_selected is null) return;
        var title = string.IsNullOrWhiteSpace(_selected.Title) ? "Untitled snippet" : _selected.Title.Trim();
        var result = MessageBox.Show(this, $"Delete '{title}'?\n\nThis cannot be undone.", "Delete snippet",
            MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
        if (result != MessageBoxResult.Yes) return;

        var removed = _selected;
        removed.PropertyChanged -= Snippet_PropertyChanged;
        _snippets.Remove(removed);
        _dirty = true;
        _snippetView?.Refresh();
        SnippetList.SelectedItem = _snippetView?.Cast<Snippet>().FirstOrDefault();
        UpdateSelectionSurface();
        RestartSaveTimer();
    }

    private async void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new SettingsWindow(_settings.Theme) { Owner = this };
        if (window.ShowDialog() != true) return;

        if (window.SelectedTheme != _settings.Theme)
        {
            _settings.Theme = window.SelectedTheme;
            ThemeService.Apply(_settings.Theme);
            MarkSettingsDirty();
        }

        switch (window.RequestedAction)
        {
            case SettingsAction.Export:
                await ExportAsync();
                break;
            case SettingsAction.Import:
                await ImportAsync();
                break;
            case SettingsAction.Reset:
                await ResetAsync();
                break;
        }
    }

    private async Task ExportAsync()
    {
        if (!await SaveNowAsync()) return;
        var dialog = new SaveFileDialog
        {
            Title = "Export QuickShelf backup",
            Filter = "QuickShelf JSON backup (*.json)|*.json",
            FileName = $"QuickShelf-backup-{DateTime.Now:yyyy-MM-dd}.json",
            AddExtension = true,
            DefaultExt = ".json"
        };
        if (dialog.ShowDialog(this) != true) return;

        try
        {
            await _store.ExportAsync(CreateSnapshot(), dialog.FileName);
            ShowNotice($"Backup exported to {dialog.FileName}");
        }
        catch (Exception exception)
        {
            ShowNotice($"QuickShelf could not export the backup. {exception.Message}");
        }
    }

    private async Task ImportAsync()
    {
        if (!await SaveNowAsync()) return;
        var dialog = new OpenFileDialog
        {
            Title = "Import QuickShelf backup",
            Filter = "QuickShelf JSON backup (*.json)|*.json",
            CheckFileExists = true,
            Multiselect = false
        };
        if (dialog.ShowDialog(this) != true) return;

        try
        {
            var candidate = await _store.ReadImportAsync(dialog.FileName);
            var response = MessageBox.Show(this,
                $"Replace your current shelf with {candidate.Snippets.Count} imported snippet{(candidate.Snippets.Count == 1 ? string.Empty : "s")}?\n\nQuickShelf will keep your current state as its last-good backup.",
                "Import backup", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (response != MessageBoxResult.Yes) return;

            await _store.SaveAsync(candidate);
            _loading = true;
            ApplyState(candidate);
            _dirty = false;
            SaveStatusText.Text = "Imported";
            ShowNotice($"Imported {candidate.Snippets.Count} snippet{(candidate.Snippets.Count == 1 ? string.Empty : "s")} successfully.");
        }
        catch (Exception exception)
        {
            ShowNotice($"QuickShelf could not import that backup. Your current shelf was not replaced. {exception.Message}");
        }
        finally
        {
            _loading = false;
            UpdateSelectionSurface();
        }
    }

    private async Task ResetAsync()
    {
        var response = MessageBox.Show(this,
            "Reset QuickShelf on this PC?\n\nThis deletes all QuickShelf snippets and local backups. Export first if you may need them later.",
            "Reset QuickShelf", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
        if (response != MessageBoxResult.Yes) return;

        try
        {
            await _store.ResetAsync();
            _loading = true;
            ApplyState(new AppState());
            _dirty = false;
            SaveStatusText.Text = "Reset complete";
            ShowNotice("QuickShelf was reset. Local snippets and QuickShelf-owned backups were removed from this PC.");
        }
        catch (Exception exception)
        {
            ShowNotice($"QuickShelf could not complete the reset. {exception.Message}");
        }
        finally
        {
            _loading = false;
            UpdateSelectionSurface();
        }
    }

    private async void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.N)
        {
            NewSnippet_Click(this, new RoutedEventArgs());
            e.Handled = true;
        }
        else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.F)
        {
            SearchBox.Focus();
            SearchBox.SelectAll();
            e.Handled = true;
        }
        else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.S)
        {
            await SaveNowAsync();
            e.Handled = true;
        }
        else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.OemComma)
        {
            SettingsButton_Click(this, new RoutedEventArgs());
            e.Handled = true;
        }
    }

    private async void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        if (_allowClose || !_dirty) return;
        e.Cancel = true;
        if (await SaveNowAsync())
        {
            _allowClose = true;
            Close();
            return;
        }

        var response = MessageBox.Show(this,
            "QuickShelf could not save the latest changes. Close anyway and risk losing those unsaved changes?",
            "Unsaved changes", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
        if (response == MessageBoxResult.Yes)
        {
            _allowClose = true;
            Close();
        }
    }

    private void ShowNotice(string message)
    {
        NoticeText.Text = message;
        NoticeBorder.Visibility = Visibility.Visible;
    }

    private void DismissNotice_Click(object sender, RoutedEventArgs e)
    {
        NoticeBorder.Visibility = Visibility.Collapsed;
    }
}
