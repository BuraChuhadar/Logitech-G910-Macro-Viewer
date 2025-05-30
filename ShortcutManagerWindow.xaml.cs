using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

namespace G910_Logitech_Utilities
{
    public partial class ShortcutManagerWindow : Window
    {
        private Key _currentShortcut;
        private ModifierKeys _currentModifiers;

        public ShortcutManagerWindow(Key currentShortcut, ModifierKeys currentModifiers)
        {
            InitializeComponent();
            _currentShortcut = currentShortcut;
            _currentModifiers = currentModifiers;
            CurrentShortcutTextBox.Text = FormatShortcut(_currentShortcut, _currentModifiers);
        }

        private string FormatShortcut(Key key, ModifierKeys modifiers)
        {
            string humanReadableKey = key switch
            {
                Key.OemTilde => "Tilde (~)",
                Key.LeftCtrl => "Left Ctrl",
                Key.RightCtrl => "Right Ctrl",
                Key.LeftAlt => "Left Alt",
                Key.RightAlt => "Right Alt",
                _ => key.ToString()
            };

            List<string> modifierParts = new List<string>();
            if (modifiers.HasFlag(ModifierKeys.Control)) modifierParts.Add("Ctrl");
            if (modifiers.HasFlag(ModifierKeys.Alt)) modifierParts.Add("Alt");
            if (modifiers.HasFlag(ModifierKeys.Shift)) modifierParts.Add("Shift");
            if (modifiers.HasFlag(ModifierKeys.Windows)) modifierParts.Add("Windows");

            return string.Join(" + ", modifierParts) + (modifierParts.Count > 0 ? " + " : "") + humanReadableKey;
        }

        private void SaveShortcutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] parts = NewShortcutTextBox.Text.Split('+');
                if (parts.Length == 2 && Enum.TryParse(parts[1], out Key newKey) && Enum.TryParse(parts[0], out ModifierKeys newModifiers))
                {
                    _currentShortcut = newKey;
                    _currentModifiers = newModifiers;
                    MessageBox.Show("Shortcut updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    CurrentShortcutTextBox.Text = FormatShortcut(_currentShortcut, _currentModifiers);
                }
                else
                {
                    MessageBox.Show("Invalid shortcut format. Please use 'Modifiers+Key'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating shortcut: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public (Key, ModifierKeys) GetUpdatedShortcut()
        {
            return (_currentShortcut, _currentModifiers);
        }
    }
}
