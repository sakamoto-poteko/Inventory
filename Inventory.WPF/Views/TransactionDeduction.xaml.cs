using System.Windows;
using System.Windows.Input;

using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for TransactionDeduction.xaml
    /// </summary>
    public partial class TransactionDeduction : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;


        public TransactionDeduction()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, DeductionTransaction.MessageToken);

        }


        private void TbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Globals.Instance.PositiveIntegerRegex.IsMatch(e.Text);
        }
    }
}
