using System.Diagnostics;
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
        trayIcon.MouseClick += OnTrayIconClicked;

        UpdateDesktopNumber(VirtualDesktop.Current);
        Application.Idle += ApplicationIdle;
    }

    private void OnTrayIconClicked(object? sender, MouseEventArgs e)
    {
        // Left click: show task view
        if (e.Button == MouseButtons.Left)
        {
            ProcessStartInfo psi = new()
            {
                // ShellExecute'ing this magic GUID here opens Task View. No idea why,
                // got the GUID from https://stackoverflow.com/a/62646513.
                FileName = "shell:::{3080F90E-D7AD-11D9-BD98-0000947B0257}",
                UseShellExecute = true,
            };
            Process.Start(psi);
        }

        // Right click: show context menu. This already happens automatically, but the position
        // gets all borked. This puts it closer to where the mouse is.
        if (e.Button == MouseButtons.Right)
        {
            trayIcon.ContextMenuStrip.Show(Cursor.Position.X, Cursor.Position.Y -  trayIcon.ContextMenuStrip.Height);
        }
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