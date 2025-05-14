using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using Inventory.Models;
using Inventory.ViewModels;
using Inventory.WPF.Utils;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for AddSupplier.xaml
    /// </summary>
    public partial class AddSupplier : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public AddSupplier()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, AddSupplierViewModel.MessageToken);
        }

    }
}
