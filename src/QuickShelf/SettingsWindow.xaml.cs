using System.Windows;
using System.Windows.Controls;
using QuickShelf.Models;

namespace QuickShelf;

public enum SettingsAction
{
    None,
    Export,
    Import,
    Reset
}

public partial class SettingsWindow : Window
{
    private bool _initializing = true;

    public SettingsWindow(ThemePreference theme)
    {
        InitializeComponent();
        SelectedTheme = theme;
        SystemThemeRadio.IsChecked = theme == ThemePreference.System;
        LightThemeRadio.IsChecked = theme == ThemePreference.Light;
        DarkThemeRadio.IsChecked = theme == ThemePreference.Dark;
        _initializing = false;
    }

    public ThemePreference SelectedTheme { get; private set; }
    public SettingsAction RequestedAction { get; private set; }

    private void ThemeRadio_Checked(object sender, RoutedEventArgs e)
    {
        if (_initializing || sender is not RadioButton { Tag: string value }) return;
        if (Enum.TryParse<ThemePreference>(value, out var theme)) SelectedTheme = theme;
    }

    private void Export_Click(object sender, RoutedEventArgs e) => Complete(SettingsAction.Export);
    private void Import_Click(object sender, RoutedEventArgs e) => Complete(SettingsAction.Import);
    private void Reset_Click(object sender, RoutedEventArgs e) => Complete(SettingsAction.Reset);
    private void Done_Click(object sender, RoutedEventArgs e) => Complete(SettingsAction.None);

    private void Complete(SettingsAction action)
    {
        RequestedAction = action;
        DialogResult = true;
    }
}
