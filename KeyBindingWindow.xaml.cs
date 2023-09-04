using G910_Logitech_Utilities.libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace G910_Logitech_Utilities
{
    /// <summary>
    /// Interaction logic for AssignedGkeysWindow.xaml
    /// </summary>
    public partial class KeyBindingsWindow : Window
    {
        public KeyBindingsWindow(List<KeyBindingsInfo> KeyBindingsInfos)
        {
            InitializeComponent();
            KeyBindingsListBox.ItemsSource = KeyBindingsInfos;
        }

        public void UpdateKeyBindingsListBox()
        {
            List<KeyBindingsInfo> updatedKeyBindingsList = new LogitechDBQuery().GetKeyBindingsInfosFromGHubDatabase();
            KeyBindingsListBox.ItemsSource = updatedKeyBindingsList;
        }
    }
}
