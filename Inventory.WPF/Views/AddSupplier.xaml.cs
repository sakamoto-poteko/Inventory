using System.Windows;

using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for AddSupplier.xaml
    /// </summary>
    public partial class AddSupplier : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public AddSupplier()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, AddSupplierViewModel.MessageToken);
        }

    }
}
