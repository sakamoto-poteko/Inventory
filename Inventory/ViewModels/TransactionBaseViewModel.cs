using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Inventory.Framework;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.ViewModels
{
    public abstract class TransactionBaseViewModel : DbViewModelBase
    {
        public enum EnumTransactionType
        {
            Purchase,
            Return,
            Retrieve,
            Shrinkage
        }

        protected readonly InventoryContext _context;

        private string _comments;

        private ObservableCollection<Models.Inventory> _inventoriesList;
        private string _inventoryKeyword;

        protected int? IntQuantity
        {
            get
            {
                if (int.TryParse(_quantity, out var val))
                {
                    return val;
                }
                else
                {
                    return null;
                }
            }
        }
        private string _quantity;

        private Models.Inventory _selectedInventory;

        private string _supplierKeyword;

        private DateTime _transactionTime = DateTime.Now;

        protected TransactionBaseViewModel()
        {
            _context = VanillaInventoryContext;
            PropertyChanged += TransactionViewModel_PropertyChanged;
        }


        public EnumTransactionType TransactionType { get; protected set; }

        public string InventoryComments => SelectedInventory?.Comments;
        public Location InventoryLocation => SelectedInventory?.Location;
        public Product InventoryProduct => SelectedInventory?.Product;
        public int? InventoryQuantity => SelectedInventory?.Quantity;

        public Models.Inventory SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(InventoryComments));
                RaisePropertyChanged(nameof(InventoryLocation));
                RaisePropertyChanged(nameof(InventoryProduct));
                RaisePropertyChanged(nameof(InventoryQuantity));

                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<Models.Inventory> InventoriesList
        {
            get => _inventoriesList;
            protected set
            {
                _inventoriesList = value;
                RaisePropertyChanged();
            }
        }

        public string SupplierKeyword
        {
            get => _supplierKeyword;
            set
            {
                _supplierKeyword = value;
                RaisePropertyChanged();
            }
        }

        public string InventoryKeyword
        {
            get => _inventoryKeyword;
            set
            {
                _inventoryKeyword = value;
                RaisePropertyChanged();
            }
        }

        public string InventoryKeywordHint => "Product name or inventory unique ID";

        public string Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                RaisePropertyChanged();
            }
        }


        public DateTime TransactionTime
        {
            get => _transactionTime;
            set
            {
                _transactionTime = value;
                RaisePropertyChanged();
            }
        }

        public string Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand CommandInsertNext => new RelayCommand(ExecuteNext, CanInsert);
        public RelayCommand CommandInsertClose => new RelayCommand(ExecuteClose, CanInsert);
        public RelayCommand CommandSearchInventory => new RelayCommand(SearchInventoryKeyword);

        protected void TransactionViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(InventoryKeyword):
                    {
                        if (InventoryKeyword?.Length > 2)
                            SearchInventoryKeyword();
                        break;
                    }
            }
        }

        protected void SearchInventoryKeyword()
        {
            IQueryable<Models.Inventory> list = _context.Inventories
                .Include(i => i.Product)
                .ThenInclude(p => p.Footprint);
            if (!string.IsNullOrWhiteSpace(InventoryKeyword))
            {
                var keyword = $"%{WildcardToLike(InventoryKeyword)}%";
                list = _context.Inventories
                    .Where(i => EF.Functions.Like(i.Product.ProductName, keyword) ||
                                EF.Functions.Like(i.UniqueId, keyword));
            }

            InventoriesList = new ObservableCollection<Models.Inventory>(list);
        }

        protected abstract bool Execute();

        /// <summary>
        ///     Check for data that might be wrong
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckData();

        protected abstract bool CanInsert();

        protected virtual void ExecuteNext()
        {
            if (!CheckData())
                return;

            if (Execute())
                ClearFields();
        }

        protected virtual void ClearFields()
        {
            Comments = null;
            InventoryKeyword = null;
            InventoriesList = null;
            InventoryKeyword = null;
            Quantity = "0";
            SelectedInventory = null;
            SupplierKeyword = null;
            TransactionTime = DateTime.Now;
        }

        protected virtual void ExecuteClose()
        {
            if (!CheckData())
                return;

            if (Execute())
                CommandClose.Execute(null);
        }
    }
}