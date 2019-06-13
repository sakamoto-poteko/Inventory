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
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using Inventory.ViewModels;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for ViewLocations.xaml
    /// </summary>
    public partial class ViewLocations : Window
    {
        public ViewLocations()
        {
            InitializeComponent();
            Messenger.Default.Register<WindowMessages>(this, ViewLocationsViewModel.MessageToken,
                msg =>
                {
                    if (msg == WindowMessages.CloseWindow)
                        Close();
                });
            Messenger.Default.Register<ShowViewMessage>(this, ViewLocationsViewModel.MessageToken,
                msg =>
                {
                    if (msg.ViewToShow == ShowViewMessage.View.InventoryList)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            var window = new ShowInventories()
                            {
                                DataContext = msg.ViewModel
                            };
                            window.ShowDialog();
                        });
                    }
                });
        }
    }
}
