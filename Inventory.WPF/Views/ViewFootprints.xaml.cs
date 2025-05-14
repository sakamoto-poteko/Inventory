using System.Windows;

using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for ViewFootprints.xaml
    /// </summary>
    public partial class ViewFootprints : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public ViewFootprints()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, ViewFootprintsViewModel.MessageToken);
        }

    }
}
