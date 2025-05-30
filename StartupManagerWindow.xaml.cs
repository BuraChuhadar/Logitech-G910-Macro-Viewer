using System;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;

namespace G910_Logitech_Utilities
{
    public partial class StartupManagerWindow : Window
    {
        public StartupManagerWindow()
        {
            InitializeComponent();
        }

        private void EnableStartupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string? appName = Assembly.GetExecutingAssembly().GetName().Name;
                string appPath = Assembly.GetExecutingAssembly().Location;

                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (key == null)
                    {
                        throw new Exception("Failed to open registry key.");
                    }

                    key.SetValue(appName, $"\"{appPath}\"");
                    MessageBox.Show("Startup enabled successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error enabling startup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisableStartupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string? appName = Assembly.GetExecutingAssembly().GetName().Name;

                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (key == null)
                    {
                        throw new Exception("Failed to open registry key.");
                    }

                    if (key.GetValue(appName) != null)
                    {
                        key.DeleteValue(appName);
                        MessageBox.Show("Startup disabled successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error disabling startup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
