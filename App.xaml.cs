﻿using G910_Logitech_Utilities;
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
            string appPath = $@"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{appName}.exe";

            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (key == null)
                {
                    return;
                }
                if (enable)
                {
                    key.SetValue(appName, $"\"{appPath}\"");
                }
                else
                {
                    key.DeleteValue(appName ?? "", false);
                }
            }
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
