using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G910_Logitech_Utilities.libs
{
    public class KeyBindingsInfo
    {
        public required string Key { get; set; }
        public required MacroName KeyMacroName { get; set; }
        public required string KeyBindingsName { get; set; }

        public enum MacroName
        {
            M1,
            M2,
            M3
        }
    }

}
