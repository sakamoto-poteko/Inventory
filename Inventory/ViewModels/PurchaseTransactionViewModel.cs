using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Inventory.Framework;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
#if !WINDOWS_UWP
using System.Windows.Input;
using System.Windows;
#endif

namespace Inventory.ViewModels
{
    public class PurchaseTransactionViewModel : TransactionBaseViewModel
    {
        public static Guid MessageToken = Guid.NewGuid();

        private bool _createNewInventory;


        private ObservableCollection<Location> _locationList;

        private string _locationNameKeyword;

        private string _newUniqueId;

        private string _price = "0";

        private ObservableCollection<Product> _productList;

        private string _productNameKeyword;

        private Location _selectedLocation;

        private Product _selectedProduct;

        private Supplier _selectedSupplier;

        private ObservableCollection<Supplier> _suppliersList;

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
                if (!double.TryParse(_price, out var val))
                    return null;
                return (uint)(val * 1000.0);
            }
        }

        public string Price
        {
            get => _price;
            set
            {
                _price = value;
                RaisePropertyChanged();
            }
        }

        public bool CreateNewInventory
        {
            get => _createNewInventory;
            set
            {
                _createNewInventory = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<Product> ProductList
        {
            get => _productList;
            set
            {
                _productList = value;
                RaisePropertyChanged();
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                RaisePropertyChanged();
                if (value != null)
                    SetUniqueId();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<Location> LocationList
        {
            get => _locationList;
            set
            {
                _locationList = value;
                RaisePropertyChanged();
            }
        }

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string ProductNameKeyword
        {
            get => _productNameKeyword;
            set
            {
                _productNameKeyword = value;
                RaisePropertyChanged();
            }
        }

        public string LocationNameKeyword
        {
            get => _locationNameKeyword;
            set
            {
                _locationNameKeyword = value;
                RaisePropertyChanged();
            }
        }

        public string NewUniqueId
        {
            get => _newUniqueId;
            set
            {
                _newUniqueId = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<Supplier> SuppliersList
        {
            get => _suppliersList;
            set
            {
                _suppliersList = value;
                RaisePropertyChanged();
            }
        }

        public Supplier SelectedSupplier
        {
            get => _selectedSupplier;
            set
            {
                _selectedSupplier = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public RelayCommand CommandSearchProduct => new RelayCommand(SearchProductNameKeyword);
        public RelayCommand CommandSearchLocation => new RelayCommand(SearchLocationNameKeyword);
        public RelayCommand CommandSearchSupplier => new RelayCommand(SearchSupplierKeyword);

        private void SetUniqueId()
        {
            Debug.Assert(SelectedProduct != null, nameof(SelectedProduct) + " != null");
            NewUniqueId = SelectedProduct.Footprint == null
                ? SelectedProduct.ProductName.ToUpper()
                : $"{SelectedProduct.ProductName.ToUpper()}@{SelectedProduct.Footprint.FootprintName}";
        }

        private void SearchProductNameKeyword()
        {
            IQueryable<Product> products = _context.Products.Include(p => p.Footprint);
            if (!string.IsNullOrWhiteSpace(ProductNameKeyword))
                products = products.Where(p =>
                    EF.Functions.Like(p.ProductName, $"{WildcardToLike(ProductNameKeyword)}%"));
            ProductList = new ObservableCollection<Product>(products);
        }

        private void SearchLocationNameKeyword()
        {
            if (!string.IsNullOrWhiteSpace(LocationNameKeyword))
            {
                var locations = SearchLocationNameKeyword(_context, LocationNameKeyword);
                LocationList = new ObservableCollection<Location>(locations);
            }
        }


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
            using (var transaction = _context.Database.BeginTransaction())
            {
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
                    MessageBox.Show($"Unable to save: {err}", "Save failed", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    transaction.Rollback();
                    UndoingChangesDbContextLevel(_context);
                    return false;
                }
            }
        }

        protected override bool CheckData()
        {
            if (UintPrice == 0)
                return MessageBox.Show("Price is 0, are you sure?", "Data check",
                           MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;

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
        }

        protected override bool ShouldPromptClose()
        {
            return false;
        }
    }
}