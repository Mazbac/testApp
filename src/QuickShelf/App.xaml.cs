using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace QuickShelf;

public partial class App : Application
{
    static App()
    {
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
    }
}
