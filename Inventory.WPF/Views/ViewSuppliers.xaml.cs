using System.Windows;

using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for ViewSuppliers.xaml
    /// </summary>
    public partial class ViewSuppliers : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public ViewSuppliers()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, ViewSuppliersViewModel.MessageToken);
        }
    }
}
