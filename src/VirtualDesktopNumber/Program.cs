using VirtualDesktopNumber.UI;

namespace VirtualDesktopNumber;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.Run(new TrayContext());
    }
}
