using System.Windows;

using CommunityToolkit.Mvvm.Input;

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

        public RelayCommand CloseCommand => new(Close);

    }
}
