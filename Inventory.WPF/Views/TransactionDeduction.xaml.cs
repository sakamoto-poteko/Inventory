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
    /// Interaction logic for TransactionDeduction.xaml
    /// </summary>
    public partial class TransactionDeduction : Window
    {
        public TransactionDeduction()
        {
            InitializeComponent();
            Messenger.Default.Register<WindowMessages>(this, DeductionTransaction.MessageToken,
                msg =>
                {
                    if (msg == WindowMessages.CloseWindow)
                        Close();
                });
        }


        private void TbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Globals.Instance.PositiveIntegerRegex.IsMatch(e.Text);
        }
    }
}
