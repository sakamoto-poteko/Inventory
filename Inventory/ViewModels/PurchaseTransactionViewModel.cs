using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Inventory.Framework;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.ViewModels
{
    public partial class PurchaseTransactionViewModel : TransactionBaseViewModel
    {
        public static Guid MessageToken = Guid.NewGuid();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExecuteCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExecuteNextCommand))]
        private bool createNewInventory;

        [ObservableProperty]
        private ObservableCollection<Location> locationList;

        [ObservableProperty]
        private string locationNameKeyword;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExecuteCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExecuteNextCommand))]
        private string newUniqueId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExecuteCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExecuteNextCommand))] 
        private string price = "0";

        [ObservableProperty]
        private ObservableCollection<Product> productList;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExecuteCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExecuteNextCommand))]
        private string productNameKeyword;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExecuteCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExecuteNextCommand))]
        private Location selectedLocation;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExecuteCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExecuteNextCommand))]
        private Supplier selectedSupplier;

        [ObservableProperty]
        private ObservableCollection<Supplier> suppliersList;

        public PurchaseTransactionViewModel()
        {
            TransactionType = EnumTransactionType.Purchase;
            PropertyChanged += PurchaseTransactionViewModel_PropertyChanged;
        }

        public override Guid MsgToken => MessageToken;

        private uint? UintPrice
        {
            get
            {
                if (!double.TryParse(Price, out var val))
                    return null;
                return (uint)(val * 1000.0);
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                if (value != null)
                    SetUniqueId();
                ExecuteCloseCommand.NotifyCanExecuteChanged();
                ExecuteNextCommand.NotifyCanExecuteChanged();
            }
        }

        private void SetUniqueId()
        {
            Debug.Assert(SelectedProduct != null, nameof(SelectedProduct) + " != null");
            NewUniqueId = SelectedProduct.Footprint == null
                ? SelectedProduct.ProductName.ToUpper()
                : $"{SelectedProduct.ProductName.ToUpper()}@{SelectedProduct.Footprint.FootprintName}";
        }

        [RelayCommand]
        private void SearchProductNameKeyword()
        {
            IQueryable<Product> products = _context.Products.Include(p => p.Footprint);
            if (!string.IsNullOrWhiteSpace(ProductNameKeyword))
                products = products.Where(p =>
                    EF.Functions.Like(p.ProductName, $"{WildcardToLike(ProductNameKeyword)}%"));
            ProductList = new ObservableCollection<Product>(products);
        }

        [RelayCommand]
        private void SearchLocationNameKeyword()
        {
            if (!string.IsNullOrWhiteSpace(LocationNameKeyword))
            {
                var locations = SearchLocationNameKeyword(_context, LocationNameKeyword);
                LocationList = new ObservableCollection<Location>(locations);
            }
        }


        [RelayCommand]
        protected void SearchSupplierKeyword()
        {
            IQueryable<Supplier> list = _context.Suppliers;
            if (!string.IsNullOrWhiteSpace(SupplierKeyword))
                list = _context.Suppliers.Where(s =>
                    EF.Functions.Like(s.Name, $"%{WildcardToLike(SupplierKeyword)}%"));

            SuppliersList = new ObservableCollection<Supplier>(list);
        }

        protected void PurchaseTransactionViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(InventoryKeyword):
                    {
                        if (InventoryKeyword?.Length > 2)
                            SearchInventoryKeyword();
                        break;
                    }
                case nameof(SupplierKeyword):
                    {
                        if (!string.IsNullOrWhiteSpace(SupplierKeyword) && SupplierKeyword.Length > 2)
                            SearchSupplierKeyword();
                        break;
                    }
            }
        }

        protected override bool Execute()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Models.Inventory inventory;
                if (CreateNewInventory)
                {
                    inventory = new Models.Inventory
                    {
                        Comments = NullOrWhitespaceAsNull(Comments),
                        Location = SelectedLocation,
                        Product = SelectedProduct,
                        Quantity = 0,
                        UniqueId = NewUniqueId
                    };
                    _context.Inventories.Add(inventory);
                    _context.SaveChanges();
                }
                else
                {
                    inventory = SelectedInventory;
                }

                Debug.Assert(IntQuantity != null, nameof(IntQuantity) + " != null");
                var entity = new Transaction
                {
                    Comments = NullOrWhitespaceAsNull(Comments),
                    Direction = Transaction.InventoryDirection.Addition,
                    Inventory = inventory,
                    Price = (int?)UintPrice,
                    Quantity = IntQuantity.Value,
                    Supplier = SelectedSupplier,
                    Time = TransactionTime
                };
                inventory.Quantity += IntQuantity.Value;

                _context.Transactions.Add(entity);
                _context.Entry(inventory).State = EntityState.Modified;
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (DbUpdateException ex)
            {
                string err;
                if (IsConstraintsViolation(ex))
                    err = "record already existed";
                else
                    err = ex.InnerException?.Message ?? ex.Message;
                UniversalMessageBox.Show($"Unable to save: {err}",
                    "Save failed",
                    UniversalMessageBox.MessageBoxButton.OK,
                    UniversalMessageBox.MessageBoxImage.Error);
                transaction.Rollback();
                UndoingChangesDbContextLevel(_context);
                return false;
            }
        }

        protected override bool CheckData()
        {
            if (UintPrice == 0)
                return UniversalMessageBox.Show("Price is 0, are you sure?", "Data check",
                           UniversalMessageBox.MessageBoxButton.YesNo,
                           UniversalMessageBox.MessageBoxImage.Warning) ==
                       UniversalMessageBox.MessageBoxResult.Yes;

            return true;
        }

        protected override bool CanInsert()
        {
            var inventoryValid = SelectedSupplier != null && IntQuantity > 0 && UintPrice.HasValue;
            if (CreateNewInventory)
                return inventoryValid && SelectedProduct != null && SelectedLocation != null &&
                       !string.IsNullOrWhiteSpace(NewUniqueId);
            return inventoryValid && SelectedInventory != null;
        }

        protected override void ClearFields()
        {
            base.ClearFields();
            SelectedLocation = null;
            SelectedProduct = null;
            NewUniqueId = null;
            CommandManager.InvalidateRequerySuggested();
        }

        protected override bool ShouldPromptClose()
        {
            return false;
        }
    }
}