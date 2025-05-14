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
using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for ViewSuppliers.xaml
    /// </summary>
    public partial class ViewSuppliers : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public ViewSuppliers()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, ViewSuppliersViewModel.MessageToken);
        }
    }
}
