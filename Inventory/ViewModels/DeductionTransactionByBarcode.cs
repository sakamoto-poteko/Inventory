using Inventory.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;

namespace Inventory.ViewModels
{
    public class DeductionTransactionByBarcode : DeductionTransaction
    {
        public new static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        private string _locationKeyword;

        public string LocationKeyword
        {
            get => _locationKeyword;
            set
            {
                _locationKeyword = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void SearchInventoryByLocation()
        {
            if (!string.IsNullOrWhiteSpace(LocationKeyword))
            {
                var sections = LocationKeyword.Split('-', 2);
                IQueryable<Models.Inventory> inv = _context.Inventories.Include(i => i.Location).Include(i => i.Product)
                    .ThenInclude(p => p.Footprint);
                if (sections.Length == 1)
                {
                    inv = inv.Where(i => i.Location.LocationName == sections[0] && i.Location.LocationUnit == null);
                }
                else
                {
                    inv = inv.Where(i =>
                        i.Location.LocationName == sections[0] && i.Location.LocationUnit == sections[1]);
                }
                InventoriesList = new System.Collections.ObjectModel.ObservableCollection<Models.Inventory>(inv);
                if (InventoriesList.Count > 0)
                    SelectedInventory = InventoriesList[0];
                WeakReferenceMessenger.Default.Send(new ChangeFocusMessage { FocusTarget = ChangeFocusMessage.Target.Quantity, MessageToken = MsgToken });
            }
        }

        public new Models.Inventory SelectedInventory
        {
            get => base.SelectedInventory;
            set
            {
                base.SelectedInventory = value;
                WeakReferenceMessenger.Default.Send(new ChangeFocusMessage { FocusTarget = ChangeFocusMessage.Target.Quantity, MessageToken = MsgToken });
            }
        }

        private void InsertAndNew()
        {
            ExecuteNext();
            LocationKeyword = null;
            Quantity = "0";
            WeakReferenceMessenger.Default.Send(new ChangeFocusMessage { FocusTarget = ChangeFocusMessage.Target.Search, MessageToken = MsgToken });
        }

        public RelayCommand CommandSearchByLocation => new RelayCommand(SearchInventoryByLocation,
            () => !string.IsNullOrWhiteSpace(LocationKeyword));

        public new RelayCommand CommandInsertNext => new RelayCommand(InsertAndNew, CanInsert);
    }
}
