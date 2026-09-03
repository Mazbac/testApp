namespace QuickShelf.Models;

public enum ThemePreference
{
    System,
    Light,
    Dark
}

public sealed class AppSettings
{
    public ThemePreference Theme { get; set; } = ThemePreference.System;

    public AppSettings Clone() => new() { Theme = Theme };
}
