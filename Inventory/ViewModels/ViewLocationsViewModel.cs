using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inventory.Framework;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace Inventory.ViewModels
{
    public partial class ViewLocationsViewModel : DbQueryViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        [ObservableProperty]
        private string locationNameKeyword;


        [ObservableProperty]
        private bool locationNameKeywordEnabled = true;


        [ObservableProperty]
        private string _locationUnitKeyword;

        [ObservableProperty]
        private bool locationUnitKeywordEnabled;

        [ObservableProperty]
        private string commentsKeyword;

        [ObservableProperty]
        private bool commentsKeywordEnabled;

        [ObservableProperty]
        private ObservableCollection<Location> locations;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ViewInventoriesCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private Location selectedLocation;

        [RelayCommand(CanExecute = nameof(CanViewInventories))]
        private void ViewInventories()
        {
            var context = VanillaInventoryContext;
            var inventories = context.Inventories
                .Include(i => i.Product)
                .ThenInclude(p => p.Footprint)
                .Where(i => i.LocationId == SelectedLocation.LocationId).ToList();
            WeakReferenceMessenger.Default.Send(new ShowViewMessage
            {
                ViewModel = inventories,
                ViewToShow = ShowViewMessage.View.InventoryList,
                MessageToken = MsgToken,
            });
        }

        private bool CanViewInventories()
        {
            return SelectedLocation != null;
        }

        protected override bool ShouldPromptClose()
        {
            return false;
        }

        protected override void SearchKeyword()
        {
            var context = VanillaInventoryContext;

            IQueryable<Location> locations = context.Locations;

            if (LocationNameKeywordEnabled && !string.IsNullOrWhiteSpace(LocationNameKeyword))
                locations = locations.Where(l =>
                    EF.Functions.Like(l.LocationName, WildcardToLike(LocationNameKeyword)));

            if (LocationUnitKeywordEnabled && !string.IsNullOrWhiteSpace(LocationUnitKeyword))
                locations = locations.Where(l =>
                    EF.Functions.Like(l.LocationUnit, WildcardToLike(LocationUnitKeyword)));

            if (CommentsKeywordEnabled && !string.IsNullOrWhiteSpace(CommentsKeyword))
                locations = locations.Where(l =>
                    EF.Functions.Like(l.Comments, WildcardToLike(CommentsKeyword)));

            Locations = new ObservableCollection<Location>(locations);
        }

        protected override bool PromptDelete()
        {
            var result = UniversalMessageBox.Show(
                $@"Are you sure to delete location {SelectedLocation.LocationName}{(SelectedLocation.LocationUnit == null ? "" : ", unit ")}{SelectedLocation?.LocationUnit}?",
                "Delete", UniversalMessageBox.MessageBoxButton.YesNo, UniversalMessageBox.MessageBoxImage.Warning);
            return result == UniversalMessageBox.MessageBoxResult.Yes;
        }

        protected override IEnumerable<object> GetSelectedItems()
        {
            return new[] { SelectedLocation };
        }

        protected override void RemoveSelectedItemFromList()
        {
            Locations.Remove(SelectedLocation);
        }

        protected override void ClearSelectedItems()
        {
            SelectedLocation = null;
        }

        protected override bool CanDelete()
        {
            return SelectedLocation != null;
        }
    }
}
