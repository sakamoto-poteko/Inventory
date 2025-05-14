using System.Windows;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for InventoryManager.xaml
    /// </summary>
    public partial class InventoryManager : Window
    {
        public InventoryManager()
        {
            InitializeComponent();
        }

        private void BtnAddSupplier_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddSupplier();
            window.ShowDialog();
        }

        private void BtnAddFootprint_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddFootprint();
            window.ShowDialog();
        }

        private void BtnAddLocation_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddLocation();
            window.ShowDialog();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddProduct();
            window.ShowDialog();
        }

        private void BtnViewSuppliers_Click(object sender, RoutedEventArgs e)
        {
            var window = new ViewSuppliers();
            window.ShowDialog();
        }

        private void BtnViewLocation_Click(object sender, RoutedEventArgs e)
        {
            var window = new ViewLocations();
            window.ShowDialog();
        }

        private void BtnViewFootprint_Click(object sender, RoutedEventArgs e)
        {
            var window = new ViewFootprints();
            window.ShowDialog();
        }

        private void BtnViewProduct_Click(object sender, RoutedEventArgs e)
        {
            var window = new ViewProducts();
            window.ShowDialog();
        }

        private void BtnPurchase_Click(object sender, RoutedEventArgs e)
        {
            var window = new TransactionPurchase();
            window.ShowDialog();
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            var window = new TransactionReturn();
            window.ShowDialog();
        }

        private void BtnRetrieve_Click(object sender, RoutedEventArgs e)
        {
            var window = new TransactionDeduction();
            window.ShowDialog();
        }

        private void BtnViewInventory_Click(object sender, RoutedEventArgs e)
        {
            var window = new ViewInventory();
            window.ShowDialog();
        }

        private void BtnRetrieveBarcodeScanner_Click(object sender, RoutedEventArgs e)
        {
            var window = new WPF.Views.TransactionDeductionByBarcode();
            window.ShowDialog();
        }
    }
}
