using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Inventory.ViewModels
{
    public partial class DeductionTransactionByBarcode : DeductionTransaction
    {
        public new static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        [ObservableProperty]
        private string locationKeyword;

        [RelayCommand(CanExecute = nameof(CanSearchByLocation))]
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

        private bool CanInsertShadow() => CanInsert();

        [RelayCommand(CanExecute = nameof(CanInsertShadow))]
        private void InsertAndNew()
        {
            ExecuteNextDefault();
            LocationKeyword = null;
            Quantity = "0";
            WeakReferenceMessenger.Default.Send(new ChangeFocusMessage { FocusTarget = ChangeFocusMessage.Target.Search, MessageToken = MsgToken });
        }

        public override IRelayCommand ExecuteNextCommand => InsertAndNewCommand; 


        private bool CanSearchByLocation() => !string.IsNullOrWhiteSpace(LocationKeyword);

    }
}
