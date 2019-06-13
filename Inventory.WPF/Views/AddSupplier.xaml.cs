using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using Inventory.Models;
using Inventory.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for AddSupplier.xaml
    /// </summary>
    public partial class AddSupplier : Window
    {
        public AddSupplier()
        {
            InitializeComponent();
            Messenger.Default.Register<WindowMessages>(this, AddSupplierViewModel.MessageToken,
                msg =>
                {
                    if (msg == WindowMessages.CloseWindow)
                        Close();
                });
        }
    }
}
