using System.Windows;

using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for ViewProducts.xaml
    /// </summary>
    public partial class ViewProducts : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public ViewProducts()
        {
            InitializeComponent(); 
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, ViewProductsViewModel.MessageToken);
        }
    }
}
