using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for TransactionPurchase.xaml
    /// </summary>
    public partial class TransactionPurchase : Window
    {
        public TransactionPurchase()
        {
            InitializeComponent();
            Messenger.Default.Register<WindowMessages>(this, PurchaseTransactionViewModel.MessageToken,
                msg =>
                {
                    if (msg == WindowMessages.CloseWindow)
                        Close();
                });
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
