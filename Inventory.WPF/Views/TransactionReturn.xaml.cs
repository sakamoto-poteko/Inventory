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
using CommunityToolkit.Mvvm.Messaging;
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
