using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.ViewModels
{
    public enum WindowMessages
    {
        CloseWindow
    }

    public class ShowViewMessage
    {
        public enum View
        {
            InventoryList,
        }

        public View ViewToShow { get; set; }
        public object ViewModel { get; set; }
    }
}
