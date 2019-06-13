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
    /// Interaction logic for ViewFootprints.xaml
    /// </summary>
    public partial class ViewFootprints : Window
    {
        public ViewFootprints()
        {
            InitializeComponent();
            Messenger.Default.Register<WindowMessages>(this, ViewFootprintsViewModel.MessageToken,
                msg =>
                {
                    if (msg == WindowMessages.CloseWindow)
                        Close();
                });
        }
    }
}
