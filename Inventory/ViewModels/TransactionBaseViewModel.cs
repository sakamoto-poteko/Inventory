using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Inventory.ViewModels
{
    public abstract partial class TransactionBaseViewModel : DbViewModelBase
    {
        public enum EnumTransactionType
        {
            Purchase,
            Return,
            Retrieve,
            Shrinkage
        }

        protected readonly InventoryContext _context;

        [ObservableProperty]
        private string comments;

        [ObservableProperty]
        private ObservableCollection<Models.Inventory> inventoriesList;
        
        [ObservableProperty]
        private string inventoryKeyword;

        protected int? IntQuantity
        {
            get
            {
                if (int.TryParse(Quantity, out var val))
                {
                    return val;
                }
                else
                {
                    return null;
                }
            }
        }
        
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExecuteCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExecuteNextCommand))]
        private string quantity;

        [ObservableProperty]
        private string supplierKeyword;

        [ObservableProperty]
        private DateTime transactionTime = DateTime.Now;

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

        private Models.Inventory _selectedInventory;
        public Models.Inventory SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                SetProperty(ref _selectedInventory, value);

                OnPropertyChanged(nameof(InventoryComments));
                OnPropertyChanged(nameof(InventoryLocation));
                OnPropertyChanged(nameof(InventoryProduct));
                OnPropertyChanged(nameof(InventoryQuantity));

                ExecuteCloseCommand.NotifyCanExecuteChanged();
                ExecuteNextCommand.NotifyCanExecuteChanged();
            }
        }

        public string InventoryKeywordHint => "Product name or inventory unique ID";

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

        [RelayCommand]
        protected void SearchInventoryKeyword()
        {
            IQueryable<Models.Inventory> list = _context.Inventories
                .Include(i => i.Product)
                .ThenInclude(p => p.Footprint)
                .Include(i => i.Location);
            if (!string.IsNullOrWhiteSpace(InventoryKeyword))
            {
                var keyword = $"%{WildcardToLike(InventoryKeyword)}%";
                list = list.Where(i => EF.Functions.Like(i.Product.ProductName, keyword) ||
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

        [RelayCommand(CanExecute = nameof(CanInsert))]
        protected virtual void ExecuteNextDefault()
        {
            if (!CheckData())
                return;

            if (Execute())
                ClearFields();
        }

        public virtual IRelayCommand ExecuteNextCommand => ExecuteNextDefaultCommand;

        [RelayCommand]
        protected virtual void ClearFields()
        {
            Comments = null;
            InventoryKeyword = null;
            InventoriesList = null;
            InventoryKeyword = null;
            SelectedInventory = null;
            SupplierKeyword = null;
            TransactionTime = DateTime.Now;
            CommandManager.InvalidateRequerySuggested();
        }

        [RelayCommand(CanExecute = nameof(CanInsert))]
        protected virtual void ExecuteCloseDefault()
        {
            if (!CheckData())
                return;

            if (Execute())
                CloseCommand.Execute(null);
        }

        public virtual IRelayCommand ExecuteCloseCommand => ExecuteNextDefaultCommand;

        protected IQueryable<Location> SearchLocationNameKeyword(InventoryContext context, string keyword)
        {
            var locKeyWords = keyword.Split('-', 2);

            IQueryable<Location> locations = context.Locations;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                locations = locations.Where(l =>
                    EF.Functions.Like(l.LocationName, $"{WildcardToLike(locKeyWords[0])}%"));
                if (locKeyWords.Length == 2)
                {
                    locations = locations.Where(l =>
                        EF.Functions.Like(l.LocationUnit, $"{WildcardToLike(locKeyWords[1])}%"));
                }
            }
            return locations;
        }
    }
}