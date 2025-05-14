using System.Windows;

using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for AddFootprint.xaml
    /// </summary>
    public partial class AddFootprint : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;
        
        public AddFootprint()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, AddFootprintViewModel.MessageToken);
        }
    }
}
