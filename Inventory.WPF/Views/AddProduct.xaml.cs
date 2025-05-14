using System.Windows;

using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public AddProduct()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, AddProductViewModel.MessageToken);
        }

    }
}
