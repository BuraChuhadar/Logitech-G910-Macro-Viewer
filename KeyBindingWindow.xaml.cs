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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace G910_Logitech_Utilities
{
    /// <summary>
    /// Interaction logic for AssignedGkeysWindow.xaml
    /// </summary>
    public partial class KeyBindingsWindow : Window
    {
        List<KeyBindingsInfo> currentKeyBindings = new List<KeyBindingsInfo>();
        public KeyBindingsWindow(List<KeyBindingsInfo> KeyBindingsInfos)
        {
            InitializeComponent();
            currentKeyBindings.Clear();
            currentKeyBindings.AddRange(KeyBindingsInfos);
            this.DataContext = currentKeyBindings.Where(c => c.KeyMacroName == KeyBindingsInfo.MacroName.M1).ToList();
        }

        public void UpdateKeyBindingsListBox()
        {
            currentKeyBindings.Clear();
            currentKeyBindings.AddRange(new LogitechDBQuery().GetKeyBindingsInfosFromGHubDatabase());
            this.DataContext = currentKeyBindings.Where(c => c.KeyMacroName == KeyBindingsInfo.MacroName.M1).ToList();
        }

        private void btnM_Click(object sender, RoutedEventArgs e)
        {
            StartFadeOutAnimation();
            var currentButton = (Button)sender;
            var selectedMacro = Enum.TryParse(currentButton.Tag.ToString(), out KeyBindingsInfo.MacroName keyMacroName);
            this.DataContext = currentKeyBindings.Where(c => c.KeyMacroName == keyMacroName).ToList();
            StartFadeInAnimation();
        }

        private void StartFadeOutAnimation()
        {
            Storyboard fadeOutStoryboard = this.Resources["FadeOutAnimation"] as Storyboard;

            foreach (var child in KeyBindingCanvas.Children)
            {
                if (child is TextBlock textBlock)
                {
                    fadeOutStoryboard?.Begin(textBlock, true);
                }
            }
        }
        private void StartFadeInAnimation()
        {
            Storyboard fadeInStoryboard = this.Resources["FadeInAnimation"] as Storyboard;

            foreach (var child in KeyBindingCanvas.Children)
            {
                if (child is TextBlock textBlock)
                {
                    fadeInStoryboard?.Begin(textBlock, true);
                }
            }
        }
    }
}
