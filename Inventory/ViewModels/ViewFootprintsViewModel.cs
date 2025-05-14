using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inventory.Framework;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;

#if !WINDOWS_UWP
#endif

namespace Inventory.ViewModels
{
    public partial class ViewFootprintsViewModel : DbQueryViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SearchKeywordCommand))]
        private string footprintNameKeyword;

        [ObservableProperty]
        private bool footprintNameKeywordEnabled = true;

        [ObservableProperty]
        private string footprintCommentKeyword;

        [ObservableProperty]
        private bool footprintCommentKeywordEnabled;

        [ObservableProperty]
        private ObservableCollection<Footprint> footprints;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private Footprint selectedFootprint;

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
