using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace G910_Logitech_Utilities.libs
{

    public class KeyboardHookEventArgs : EventArgs
    {
        public Key Key { get; }

        public bool Handled { get; set; }

        public KeyboardHookEventArgs(Key key)
        {
            Key = key;
        }
    }

}
