using System;

namespace Inventory.ViewModels
{
    public class WindowMessage
    {
        public enum Type
        {
            CloseWindow,
        }

        public Type MessageType { get; set; }

        public Guid MessageToken { get; set; }
    }

    public class ShowViewMessage
    {
        public enum View
        {
            InventoryList,
        }

        public View ViewToShow { get; set; }
        public object ViewModel { get; set; }

        public Guid MessageToken { get; set; }
    }

    public class ChangeFocusMessage
    {
        public enum Target
        {
            Search,
            Quantity,
        }

        public Target FocusTarget { get; set; }

        public Guid MessageToken { get; set; }
    }

}
