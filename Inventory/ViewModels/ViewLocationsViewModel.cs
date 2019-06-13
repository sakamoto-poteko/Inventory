using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ViewLocationsViewModel : DbQueryViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        private string _locationNameKeyword;

        public string LocationNameKeyword
        {
            get => _locationNameKeyword;
            set
            {
                _locationNameKeyword = value;
                RaisePropertyChanged();
            }
        }

        private bool _locationNameKeywordEnabled = true;

        public bool LocationNameKeywordEnabled
        {
            get => _locationNameKeywordEnabled;
            set
            {
                _locationNameKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }


        private string _locationUnitKeyword;

        public string LocationUnitKeyword
        {
            get => _locationUnitKeyword;
            set
            {
                _locationUnitKeyword = value;
                RaisePropertyChanged();
            }
        }

        private bool _locationUnitKeywordEnabled;

        public bool LocationUnitKeywordEnabled
        {
            get => _locationUnitKeywordEnabled;
            set
            {
                _locationUnitKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        private string _commentsKeyword;

        public string CommentsKeyword
        {
            get => _commentsKeyword;
            set
            {
                _commentsKeyword = value;
                RaisePropertyChanged();
            }
        }

        private bool _commentsKeywordEnabled;

        public bool CommentsKeywordEnabled
        {
            get => _commentsKeywordEnabled;
            set
            {
                _commentsKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Location> _locations;

        public ObservableCollection<Location> Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                RaisePropertyChanged();
            }
        }

        private Location _selectedLocation;

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

        private void ViewInventories()
        {
            var context = VanillaInventoryContext;
            var inventories = context.Inventories
                .Include(i => i.Product)
                .ThenInclude(p => p.Footprint)
                .Where(i => i.LocationId == SelectedLocation.LocationId).ToList();
            MessengerInstance.Send(new ShowViewMessage
            {
                ViewModel = inventories,
                ViewToShow = ShowViewMessage.View.InventoryList
            }, MsgToken);
        }

        private bool CanViewInventories()
        {
            return SelectedLocation != null;
        }

        public RelayCommand CommandViewInventories => new RelayCommand(ViewInventories, CanViewInventories);

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
            var result = MessageBox.Show(
                $@"Are you sure to delete location {SelectedLocation.LocationName}{(SelectedLocation.LocationUnit == null ? "" : ", unit ")}{SelectedLocation?.LocationUnit}?",
                "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes;
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
