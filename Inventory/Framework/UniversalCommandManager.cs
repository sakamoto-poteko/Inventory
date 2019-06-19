using System;
using System.Collections.Generic;
using System.Text;

#if !WINDOWS_UWP
using System.Windows.Input;
#endif

namespace Inventory.Framework
{
    public class UniversalCommandManager
    {

        public static void InvalidateRequerySuggested()
        {
#if WINDOWS_UWP

#else
            CommandManager.InvalidateRequerySuggested();
#endif
        }
    }
}
