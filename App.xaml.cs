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

namespace G910_Macro_Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {

        public App()
        {
        }

        private void AddToStartup()
        {
            string startupFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "G910 Macro Viewer.lnk");
            if (!File.Exists(startupFolderPath))
            {
                string appPath = Assembly.GetExecutingAssembly().Location;
                IWshRuntimeLibrary.WshShell wsh = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)wsh.CreateShortcut(startupFolderPath);
                shortcut.TargetPath = appPath;
                shortcut.Save();
            }
        }

        private NotifyIcon _notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Add your application to Windows startup
            AddToStartup();

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
