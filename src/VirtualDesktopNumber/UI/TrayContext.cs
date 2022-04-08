using System.Resources;
using VirtualDesktopNumber.Helpers;
using WindowsDesktop;

namespace VirtualDesktopNumber.UI;

public class TrayContext : ApplicationContext
{
    private readonly NotifyIcon trayIcon;
    private readonly ResourceManager resourceManager = Properties.Resources.ResourceManager;

    public TrayContext()
    {
        trayIcon = new NotifyIcon()
        {
            Text = "Virtual Desktop Number",
            ContextMenuStrip = GetContextMenuStrip(),
            Visible = true
        };

        UpdateDesktopNumber(VirtualDesktop.Current);
        Application.Idle += ApplicationIdle;
    }

    private void ApplicationIdle(object? sender, EventArgs e)
    {
        Application.Idle -= ApplicationIdle;
        VirtualDesktop.CurrentChanged += OnCurrentDesktopChanged;
    }

    private void OnCurrentDesktopChanged(object? sender, VirtualDesktopChangedEventArgs e)
    {
        UpdateDesktopNumber(e.NewDesktop);
    }

    private void UpdateDesktopNumber(VirtualDesktop desktop)
    {
        SetIcon(desktop.GetNumber());
    }

    private void SetIcon(int desktopNumber)
    {
        string windowsTheme = ThemingHelpers.IsLightTheme() ? "Light" : "Dark";
        trayIcon.Icon = resourceManager.GetObject($"TrayIcon{windowsTheme}{desktopNumber}") as Icon;
    }

    private ContextMenuStrip GetContextMenuStrip()
    {
        ContextMenuStrip context = new();
        ToolStripMenuItem itemExit = new("Exit", null, (obj, args) => Exit());
        context.Items.Add(itemExit);

        return context;
    }

    private void Exit()
    {
        trayIcon.Visible = false;
        Application.Exit();
    }
}