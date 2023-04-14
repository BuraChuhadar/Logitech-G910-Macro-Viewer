using G910_Macro_Viewer.libs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Reflection;
using NLog;
using Microsoft.Win32;

namespace G910_Macro_Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error(e.Exception, "An unhandled exception occurred in the dispatcher.");
            e.Handled = true; // Set this to 'true' if you want to prevent the application from closing.
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Logger.Fatal(ex, "An unhandled exception occurred in the application domain.");
            }
            else
            {
                Logger.Fatal("An unhandled exception occurred in the application domain, but the exception object is not of type 'System.Exception'.");
            }
        }

        private void SetStartup(bool enable)
        {
            string? appName = Assembly.GetExecutingAssembly().GetName().Name;
            string appPath = $@"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{appName}.exe";

            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if(key == null)
                {
                    return;
                }
                if (enable)
                {
                    key.SetValue(appName, $"\"{appPath}\"");
                }
                else
                {
                    key.DeleteValue(appName, false);
                }
            }
        }



        private NotifyIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Add your application to Windows startup
            SetStartup(true);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Hide();

            // Set up NotifyIcon
            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon("App.ico"),
                Visible = true
            };

            // Set up context menu for the NotifyIcon
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += ExitItem_Click;
            _notifyIcon.ContextMenuStrip.Items.Add(exitItem);
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
