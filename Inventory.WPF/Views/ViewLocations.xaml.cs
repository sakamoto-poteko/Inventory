using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for ViewLocations.xaml
    /// </summary>
    public partial class ViewLocations : Window, IRecipient<ShowViewMessage>
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public ViewLocations()
        {
            InitializeComponent(); 
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, ViewLocationsViewModel.MessageToken);
           
            WeakReferenceMessenger.Default.Register<ShowViewMessage>(this);
        }

        public void Receive(ShowViewMessage message)
        {
            if (message.MessageToken == ViewLocationsViewModel.MessageToken && message.ViewToShow == ShowViewMessage.View.InventoryList)
            {
                Dispatcher.Invoke(() =>
                {
                    var window = new ShowInventories()
                    {
                        DataContext = message.ViewModel
                    };
                    window.ShowDialog();
                });
            }
        }
    }
}
