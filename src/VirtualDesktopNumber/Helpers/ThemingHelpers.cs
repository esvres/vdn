using Microsoft.Win32;

namespace VirtualDesktopNumber.Helpers;

public static class ThemingHelpers
{
    public static bool IsLightTheme()
    {
        object? reg = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "SystemUsesLightTheme", null);
        bool isLightTheme = Convert.ToBoolean(reg);

        return isLightTheme;
    }
}
