using G910_Macro_Viewer.libs;
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
using System.Diagnostics;
using System.Windows.Threading;
using NLog;

namespace G910_Macro_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyboardHook _keyboardHook;
        private bool _keyCombinationPressed;
        private DateTime _winKeyDownTime;
        private DispatcherTimer _timer;
        private MacroWindow _macroWindow;
        private bool _macroWindowShown;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            InitializeComponent();
            Hide();

            _keyboardHook = new KeyboardHook();
            _keyboardHook.KeyDown += _keyboardHook_KeyDown;
            _keyboardHook.Hook();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
            _timer.Tick += _timer_Tick;
            Logger.Info("Initilization Completed");
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            CloseMacroWindow();
            _macroWindowShown = true;
        }

        private void _keyboardHook_KeyDown(object? sender, KeyboardHookEventArgs e)
        {
            Logger.Info("Key Down Event Triggered");
            if (e.Key == Key.OemTilde && (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt)) || (Keyboard.IsKeyDown(Key.RightCtrl) && Keyboard.IsKeyDown(Key.RightAlt)))
            {
                ShowMacroWindow();
                _timer.Start();
                e.Handled = true;
            }
            Logger.Info("Key Down Event Triggered Completed");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _keyboardHook.Unhook();
        }

        private void CloseMacroWindow()
        {
            _macroWindow.Hide();
        }


        private void ShowMacroWindow()
        {
            try
            {
                Logger.Info("Show Macro Window");
                if (!_macroWindowShown)
                {


                    if (_macroWindow == null)
                    {
                        Logger.Info("Retrieve Macros");
                        List<MacroInfo> macroInfos = new LogitechDBQuery().GetMacroInfosFromGHubDatabase();
                        _macroWindow = new MacroWindow(macroInfos);
                        Logger.Info("Retrieve Macros Completed");
                    }
                }
                else
                {
                    Logger.Info("Update Macros");
                    _macroWindow.UpdateMacroListBox();
                    Logger.Info("Update Macros Completed");
                }
                double screenWidth = SystemParameters.WorkArea.Width;
                double screenHeight = SystemParameters.WorkArea.Height;
                _macroWindow.Top = screenHeight - _macroWindow.Height; // Adjust the offset above the Start menu as needed.
                _macroWindow.Left = screenWidth - _macroWindow.Width;
                _macroWindow.Show();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            
        }
    }
}
