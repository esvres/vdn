using WindowsDesktop;

namespace VirtualDesktopNumber.Helpers;

public static class VirtualDesktopExtensions
{
    public static int GetNumber(this VirtualDesktop desktop)
    {
        VirtualDesktop[] desktops = VirtualDesktop.GetDesktops();

        for (int i = 0; i < desktops.Length; i++)
        {
            if (desktop == desktops[i])
            {
                return i + 1;
            }
        };

        return 0;
    }
}
