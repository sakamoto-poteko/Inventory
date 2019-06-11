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
        private readonly Regex _priceRegex = new Regex((string) Application.Current.Resources["PriceRegexString"], RegexOptions.Compiled);
        private readonly Regex _positiveIntergerRegex = new Regex((string) Application.Current.Resources["NonNegativeIntegerRegexString"], RegexOptions.Compiled);

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
            e.Handled = !_priceRegex.IsMatch(e.Text);
        }

        private void TbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_positiveIntergerRegex.IsMatch(e.Text);

        }
    }
}
