using System.Windows;
using System.Windows.Input;

using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for TransactionReturn.xaml
    /// </summary>
    public partial class TransactionReturn : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public TransactionReturn()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, ReturnTransactionViewModel.MessageToken);
        }

        private void TbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Globals.Instance.PositiveIntegerRegex.IsMatch(e.Text);
        }
    }
}
