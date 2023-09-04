using G910_Logitech_Utilities.libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using log4net.Repository.Hierarchy;
using log4net;
using log4net.Core;
using System.Reflection;

namespace G910_Logitech_Utilities
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyboardHook _keyboardHook;
        private DispatcherTimer _timer;
        private KeyBindingsWindow _KeyBindingsWindow;
        private bool _KeyBindingsWindowShown;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(type: MethodBase.GetCurrentMethod().DeclaringType);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        public MainWindow()
        {
            InitializeComponent();
            Hide();

            _keyboardHook = new KeyboardHook();
            _keyboardHook.KeyDown += _keyboardHook_KeyDown;
            _keyboardHook.Hook();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            _timer.Tick += _timer_Tick;
            Logger.Info(message: "Initilization Completed", null);

            Logger.Info("Retrieve KeyBindings", null);
            List<KeyBindingsInfo> KeyBindingsInfos = new LogitechDBQuery().GetKeyBindingsInfosFromGHubDatabase();
            _KeyBindingsWindow = new KeyBindingsWindow(KeyBindingsInfos);
            Logger.Info("Retrieve KeyBindings Completed", null);
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            CloseKeyBindingsWindow();
            _KeyBindingsWindowShown = true;
        }

        private void _keyboardHook_KeyDown(object? sender, KeyboardHookEventArgs e)
        {
            Logger.Info("Key Down Event Triggered", null);
            if (e.Key == Key.OemTilde && (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt)) || (Keyboard.IsKeyDown(Key.RightCtrl) && Keyboard.IsKeyDown(Key.RightAlt)))
            {
                ShowKeyBindingsWindow();
                _timer.Start();
                e.Handled = true;
            }
            Logger.Info("Key Down Event Triggered Completed", null);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _keyboardHook.Unhook();
        }

        private void CloseKeyBindingsWindow()
        {
            _KeyBindingsWindow.Hide();
        }


        private void ShowKeyBindingsWindow()
        {
            try
            {
                Logger.Info("Show KeyBindings Window", null);
                if (!_KeyBindingsWindowShown)
                {


                    if (_KeyBindingsWindow == null)
                    {
                        Logger.Info("Retrieve KeyBindings", null);
                        List<KeyBindingsInfo> KeyBindingsInfos = new LogitechDBQuery().GetKeyBindingsInfosFromGHubDatabase();
                        _KeyBindingsWindow = new KeyBindingsWindow(KeyBindingsInfos);
                        Logger.Info("Retrieve KeyBindings Completed", null);
                    }
                }
                else
                {
                    Logger.Info("Update KeyBindings", null);
                    _KeyBindingsWindow.UpdateKeyBindingsListBox();
                    Logger.Info("Update KeyBindings Completed", null);
                }
                double screenWidth = SystemParameters.WorkArea.Width;
                double screenHeight = SystemParameters.WorkArea.Height;
                _KeyBindingsWindow.Top = screenHeight - _KeyBindingsWindow.Height; // Adjust the offset above the Start menu as needed.
                _KeyBindingsWindow.Left = screenWidth - _KeyBindingsWindow.Width;
                _KeyBindingsWindow.Show();
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred", ex);
            }
            
        }
    }
}
