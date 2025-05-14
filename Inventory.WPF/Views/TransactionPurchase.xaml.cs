using System.Windows;
using System.Windows.Input;

using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for TransactionPurchase.xaml
    /// </summary>
    public partial class TransactionPurchase : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public TransactionPurchase()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, PurchaseTransactionViewModel.MessageToken);
        }

        private void TbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Globals.Instance.PriceRegex.IsMatch(e.Text);
        }

        private void TbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Globals.Instance.PositiveIntegerRegex.IsMatch(e.Text);
        }

    }
}
