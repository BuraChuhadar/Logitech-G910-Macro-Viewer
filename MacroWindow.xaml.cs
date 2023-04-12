using G910_Macro_Viewer.libs;
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

namespace G910_Macro_Viewer
{
    /// <summary>
    /// Interaction logic for AssignedGkeysWindow.xaml
    /// </summary>
    public partial class MacroWindow : Window
    {
        public MacroWindow(List<MacroInfo> macroInfos)
        {
            InitializeComponent();
            MacroListBox.ItemsSource = macroInfos;
        }

        public void UpdateMacroListBox()
        {
            List<MacroInfo> updatedMacroList = new LogitechDBQuery().GetMacroInfosFromGHubDatabase();
            MacroListBox.ItemsSource = updatedMacroList;
        }
    }
}
