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

    public enum ChangeFocusMessage
    {
        FocusToSearch,
        FocusToQuantity,
    }
}
