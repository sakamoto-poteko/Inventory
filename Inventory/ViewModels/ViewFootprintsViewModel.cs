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
    public class ViewFootprintsViewModel : DbQueryViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        private string _footprintNameKeyword;

        public string FootprintNameKeyword
        {
            get => _footprintNameKeyword;
            set
            {
                _footprintNameKeyword = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool _footprintNameKeywordEnabled = true;

        public bool FootprintNameKeywordEnabled
        {
            get => _footprintNameKeywordEnabled;
            set
            {
                _footprintNameKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        private string _footprintCommentKeyword;

        public string FootprintCommentKeyword
        {
            get => _footprintCommentKeyword;
            set
            {
                _footprintCommentKeyword = value;
                RaisePropertyChanged();
            }
        }

        private bool _footprintCommentKeywordEnabled;

        public bool FootprintCommentKeywordEnabled
        {
            get => _footprintCommentKeywordEnabled;
            set
            {
                _footprintCommentKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Footprint> _footprints;
        private Footprint _selectedFootprint;

        public ObservableCollection<Footprint> Footprints
        {
            get => _footprints;
            set
            {
                _footprints = value;
                RaisePropertyChanged();
            }
        }

        public Footprint SelectedFootprint
        {
            get => _selectedFootprint;
            set
            {
                _selectedFootprint = value;
                RaisePropertyChanged();
            }
        }

        protected override bool ShouldPromptClose()
        {
            return false;
        }

        protected override void SearchKeyword()
        {
            var context = VanillaInventoryContext;
            IQueryable<Footprint> footprints = context.Footprints;

            if (FootprintNameKeywordEnabled && !string.IsNullOrWhiteSpace(FootprintNameKeyword))
                footprints = footprints.Where(f =>
                    EF.Functions.Like(f.FootprintName, WildcardToLike(FootprintNameKeyword)));
            if (FootprintCommentKeywordEnabled && !string.IsNullOrWhiteSpace(FootprintCommentKeyword))
                footprints = footprints.Where(f =>
                    EF.Functions.Like(f.Comments, WildcardToLike(FootprintCommentKeyword)));

            Footprints = new ObservableCollection<Footprint>(footprints);
        }

        protected override bool PromptDelete()
        {
            var result = UniversalMessageBox.Show(
                $@"Are you sure to delete footprint {SelectedFootprint.FootprintName}?",
                "Delete", UniversalMessageBox.MessageBoxButton.YesNo, UniversalMessageBox.MessageBoxImage.Warning);
            return result == UniversalMessageBox.MessageBoxResult.Yes;
        }

        protected override IEnumerable<object> GetSelectedItems()
        {
            return new[] { SelectedFootprint };
        }

        protected override void RemoveSelectedItemFromList()
        {
            Footprints.Remove(SelectedFootprint);
        }

        protected override void ClearSelectedItems()
        {
            SelectedFootprint = null;
        }

        protected override bool CanDelete()
        {
            return SelectedFootprint != null;
        }
    }
}
