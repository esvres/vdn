using WindowsDesktop;

namespace VirtualDesktopNumber.Helpers;

public static class VirtualDesktopExtensions
{
    public static int GetNumber(this VirtualDesktop desktop)
    {
        return Array.FindIndex(VirtualDesktop.GetDesktops(), d => d == desktop) + 1;
    }
}
