using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using QuickShelf.Models;

namespace QuickShelf.Services;

public static class ThemeService
{
    public static void Apply(ThemePreference preference)
    {
        if (SystemParameters.HighContrast)
        {
            ApplyHighContrast();
            return;
        }

        var dark = preference == ThemePreference.Dark ||
            (preference == ThemePreference.System && IsSystemDark());

        SetColor("WindowBackgroundBrush", dark ? "#202020" : "#F7F7F7");
        SetColor("SurfaceBrush", dark ? "#2B2B2B" : "#FFFFFF");
        SetColor("RaisedSurfaceBrush", dark ? "#323232" : "#F2F2F2");
        SetColor("BorderBrush", dark ? "#555555" : "#CFCFCF");
        SetColor("TextBrush", dark ? "#F5F5F5" : "#1A1A1A");
        SetColor("MutedTextBrush", dark ? "#C5C5C5" : "#5A5A5A");
        SetColor("AccentBrush", dark ? "#60CDFF" : "#0F6CBD");
        SetColor("AccentTextBrush", dark ? "#001A24" : "#FFFFFF");
        SetColor("SelectionBrush", dark ? "#173B4A" : "#E5F1FB");
        SetColor("DangerBrush", dark ? "#FF99A4" : "#B42318");
        SetColor("InputBrush", dark ? "#292929" : "#FFFFFF");
    }

    public static bool IsSystemDark()
    {
        var value = Registry.GetValue(
            @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
            "AppsUseLightTheme", 1);
        return value is int intValue && intValue == 0;
    }

    private static void ApplyHighContrast()
    {
        SetBrush("WindowBackgroundBrush", SystemColors.WindowBrush);
        SetBrush("SurfaceBrush", SystemColors.WindowBrush);
        SetBrush("RaisedSurfaceBrush", SystemColors.ControlBrush);
        SetBrush("BorderBrush", SystemColors.ActiveBorderBrush);
        SetBrush("TextBrush", SystemColors.WindowTextBrush);
        SetBrush("MutedTextBrush", SystemColors.GrayTextBrush);
        SetBrush("AccentBrush", SystemColors.HighlightBrush);
        SetBrush("AccentTextBrush", SystemColors.HighlightTextBrush);
        SetBrush("SelectionBrush", SystemColors.HighlightBrush);
        SetBrush("DangerBrush", SystemColors.WindowTextBrush);
        SetBrush("InputBrush", SystemColors.WindowBrush);
    }

    private static void SetColor(string key, string color) =>
        SetBrush(key, new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)));

    private static void SetBrush(string key, Brush brush)
    {
        Application.Current.Resources[key] = brush;
    }
}
