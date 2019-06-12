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
using Inventory.Framework;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for ShowInventories.xaml
    /// </summary>
    public partial class ShowInventories : Window
    {
        public ShowInventories()
        {
            InitializeComponent();
        }

        public RelayCommand CommandClose => new RelayCommand(Close);

    }
}
