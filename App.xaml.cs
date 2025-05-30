using G910_Logitech_Utilities;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;

namespace G910_Logitech_Utilities
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(type: MethodBase.GetCurrentMethod().DeclaringType);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            // Set up NotifyIcon
            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon("App.ico")
            };
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error("An unhandled exception occurred in the dispatcher.", e.Exception);
            e.Handled = true; // Set this to 'true' if you want to prevent the application from closing.
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Logger.Fatal( "An unhandled exception occurred in the application domain.", ex);
            }
            else
            {
                Logger.Fatal("An unhandled exception occurred in the application domain, but the exception object is not of type 'System.Exception'.", null);
            }
        }

        private void SetStartup(bool enable)
        {
            string? appName = Assembly.GetExecutingAssembly().GetName().Name;
            if (string.IsNullOrEmpty(appName))
            {
                throw new Exception("Application name cannot be null or empty.");
            }

            string appPath = Assembly.GetExecutingAssembly().Location;

            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (key == null)
                    {
                        throw new Exception("Failed to open registry key.");
                    }

                    if (enable)
                    {
                        key.SetValue(appName, $"\"{appPath}\"");
                        Console.WriteLine("Startup enabled successfully.");
                    }
                    else
                    {
                        if (key.GetValue(appName) != null)
                        {
                            key.DeleteValue(appName);
                            Console.WriteLine("Startup disabled successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting startup: {ex.Message}");
            }
        }

        private void OpenStartupManager()
        {
            StartupManagerWindow startupManagerWindow = new StartupManagerWindow();
            startupManagerWindow.ShowDialog();
        }

        private Key _shortcutKey = Key.OemTilde; // Default shortcut key
        private ModifierKeys _shortcutModifiers = ModifierKeys.Control | ModifierKeys.Alt; // Default modifiers

        private string GetHumanReadableKey(Key key)
        {
            return key switch
            {
                Key.OemTilde => "Tilde (~)",
                Key.LeftCtrl => "Left Ctrl",
                Key.RightCtrl => "Right Ctrl",
                Key.LeftAlt => "Left Alt",
                Key.RightAlt => "Right Alt",
                _ => key.ToString()
            };
        }

        private string GetHumanReadableModifiers(ModifierKeys modifiers)
        {
            List<string> parts = new List<string>();
            if (modifiers.HasFlag(ModifierKeys.Control)) parts.Add("Ctrl");
            if (modifiers.HasFlag(ModifierKeys.Alt)) parts.Add("Alt");
            if (modifiers.HasFlag(ModifierKeys.Shift)) parts.Add("Shift");
            if (modifiers.HasFlag(ModifierKeys.Windows)) parts.Add("Windows");
            return string.Join(" + ", parts);
        }

        private void OpenShortcutManager()
        {
            ShortcutManagerWindow shortcutManagerWindow = new ShortcutManagerWindow(_shortcutKey, _shortcutModifiers);
            shortcutManagerWindow.ShowDialog();
            (_shortcutKey, _shortcutModifiers) = shortcutManagerWindow.GetUpdatedShortcut();
        }

        private NotifyIcon _notifyIcon;

        void App_Startup(object sender, StartupEventArgs e)
        {

            // Add your application to Windows startup
            SetStartup(true);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Hide();

            

            // Set up context menu for the NotifyIcon
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += ExitItem_Click;
            _notifyIcon.ContextMenuStrip.Items.Add(exitItem);

            // Add a menu item to open the Startup Manager
            ToolStripMenuItem startupManagerItem = new ToolStripMenuItem("Startup Manager");
            startupManagerItem.Click += (s, args) => OpenStartupManager();
            _notifyIcon.ContextMenuStrip.Items.Add(startupManagerItem);

            // Add a menu item to open the Shortcut Manager
            ToolStripMenuItem shortcutManagerItem = new ToolStripMenuItem("Shortcut Manager");
            shortcutManagerItem.Click += (s, args) => OpenShortcutManager();
            _notifyIcon.ContextMenuStrip.Items.Add(shortcutManagerItem);

            _notifyIcon.Visible = true;
            SetStartup(true);
        }

        private void ExitItem_Click(object? sender, EventArgs e)
        {
            // Close the application when the Exit menu item is clicked
            _notifyIcon.Dispose();
            MainWindow.Close();
            Current.Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
