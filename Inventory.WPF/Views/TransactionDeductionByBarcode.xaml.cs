using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using Inventory.ViewModels;

namespace Inventory.WPF.Views
{
    /// <summary>
    /// Interaction logic for TransactionDeductionByBarcode.xaml
    /// </summary>
    public partial class TransactionDeductionByBarcode : Window
    {
        public TransactionDeductionByBarcode()
        {
            InitializeComponent();
            Messenger.Default.Register<WindowMessages>(this, DeductionTransactionByBarcode.MessageToken,
                msg =>
                {
                    if (msg == WindowMessages.CloseWindow)
                        Close();
                });

            Messenger.Default.Register<ChangeFocusMessage>(this, DeductionTransactionByBarcode.MessageToken,
                msg =>
                {
                    switch (msg)
                    {
                        case ChangeFocusMessage.FocusToSearch:
                            TbLocationKeyword.Focus();
                            break;
                        case ChangeFocusMessage.FocusToQuantity:
                            TbQuantity.Focus();
                            break;
                        default:
                            break;
                    }
                });
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
