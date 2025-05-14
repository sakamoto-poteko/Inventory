using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.Messaging;
using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.WPF.Views
{
    /// <summary>
    /// Interaction logic for TransactionDeductionByBarcode.xaml
    /// </summary>
    public partial class TransactionDeductionByBarcode : Window, IRecipient<ChangeFocusMessage>
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public TransactionDeductionByBarcode()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, DeductionTransactionByBarcode.MessageToken);
            WeakReferenceMessenger.Default.Register<ChangeFocusMessage>(this);
        }

        public void Receive(ChangeFocusMessage message)
        {
            if (message.MessageToken == DeductionTransactionByBarcode.MessageToken)
            {
                switch (message.FocusTarget)
                {
                    case ChangeFocusMessage.Target.Search:
                        TbLocationKeyword.Focus();
                        break;
                    case ChangeFocusMessage.Target.Quantity:
                        TbQuantity.Focus();
                        break;
                    default:
                        break;
                }
            }
        }

        private void TbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Globals.Instance.PositiveIntegerRegex.IsMatch(e.Text))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = false;
                TbLocationKeyword.Text = string.Empty;
                TbLocationKeyword.Focus();
            }
        }
    }
}
