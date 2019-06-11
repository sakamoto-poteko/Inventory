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

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for TransactionReturn.xaml
    /// </summary>
    public partial class TransactionReturn : Window
    {
        public TransactionReturn()
        {
            InitializeComponent();
            Messenger.Default.Register<WindowMessages>(this, ReturnTransactionViewModel.MessageToken,
                msg =>
                {
                    if (msg == WindowMessages.CloseWindow)
                        Close();
                });
        }

        private void TbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !App.PositiveIntergerRegex.IsMatch(e.Text);
        }
    }
}
