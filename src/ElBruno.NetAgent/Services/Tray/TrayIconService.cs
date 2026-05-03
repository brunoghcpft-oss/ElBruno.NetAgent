using System.Drawing;
using Microsoft.Extensions.Logging;
using WinForms = System.Windows.Forms;

namespace ElBruno.NetAgent.Services.Tray;

/// <summary>
/// Manages the Windows system tray icon and context menu.
/// </summary>
public class TrayIconService : IDisposable
{
    private readonly WinForms.NotifyIcon _notifyIcon;
    private readonly ILogger<TrayIconService> _logger;
    private bool _disposed;

    public TrayIconService(ILogger<TrayIconService> logger)
    {
        _logger = logger;

        _notifyIcon = new WinForms.NotifyIcon
        {
            Visible = true,
            Text = "ElBruno.NetAgent",
            Icon = SystemIcons.Shield
        };

        _notifyIcon.ContextMenuStrip = CreateContextMenuStrip();
        _notifyIcon.DoubleClick += OnDoubleClick;

        _logger.LogInformation("Tray icon initialized");
    }

    private WinForms.ContextMenuStrip CreateContextMenuStrip()
    {
        var menuStrip = new WinForms.ContextMenuStrip();

        // Header
        menuStrip.Items.Add("ElBruno.NetAgent").Enabled = false;

        // Status (placeholder)
        var statusItem = menuStrip.Items.Add("Status: Starting");
        statusItem.Name = "StatusItem";
        statusItem.Enabled = false;

        // Separator
        menuStrip.Items.Add("-");

        // Open Status (placeholder)
        menuStrip.Items.Add("Open Status", null, (s, e) =>
        {
            _logger.LogInformation("Open Status clicked (placeholder)");
        });

        // Refresh Now
        menuStrip.Items.Add("Refresh Now", null, (s, e) =>
        {
            _logger.LogInformation("Refresh Now clicked (placeholder)");
        });

        // Separator
        menuStrip.Items.Add("-");

        // Exit
        menuStrip.Items.Add("Exit", null, (s, e) =>
        {
            _logger.LogInformation("Exit requested from tray menu");
            // Shutdown is handled by the Exit menu command in the hosted service
            Environment.Exit(0);
        });

        return menuStrip;
    }

    private void OnDoubleClick(object? sender, EventArgs e)
    {
        _logger.LogInformation("Tray icon double-clicked");
    }

    /// <summary>
    /// Updates the status text shown in the tray context menu.
    /// </summary>
    public void UpdateStatus(string status)
    {
        var statusItem = _notifyIcon.ContextMenuStrip?.Items["StatusItem"];
        if (statusItem != null)
        {
            statusItem.Text = $"Status: {status}";
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        _disposed = true;

        _logger.LogInformation("Tray icon disposed");
    }
}
