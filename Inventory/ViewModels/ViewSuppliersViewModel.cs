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
    public partial class ViewSuppliersViewModel : DbQueryViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();

        [ObservableProperty]
        private string commentsKeyword;

        [ObservableProperty]
        private bool commentsKeywordEnabled;

        [ObservableProperty]
        private string nameKeyword;

        [ObservableProperty]
        private bool nameKeywordEnabled = true;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private Supplier selectedItem;

        [ObservableProperty]
        private ObservableCollection<Supplier> suppliers;


        public override Guid MsgToken => MessageToken;

        protected override bool ShouldPromptClose()
        {
            return false;
        }

        protected override void SearchKeyword()
        {
            var context = VanillaInventoryContext;
            IQueryable<Supplier> suppliers = context.Suppliers;
            if (NameKeywordEnabled && !string.IsNullOrWhiteSpace(NameKeyword))
                suppliers = context.Suppliers.Where(s => EF.Functions.Like(s.Name, WildcardToLike(NameKeyword)));
            if (CommentsKeywordEnabled && !string.IsNullOrWhiteSpace(CommentsKeyword))
                suppliers = context.Suppliers.Where(s => EF.Functions.Like(s.Comments, WildcardToLike(CommentsKeyword)));

            Suppliers = new ObservableCollection<Supplier>(suppliers);
        }

        protected override bool PromptDelete()
        {
            var result = UniversalMessageBox.Show(
                $"Are you sure to delete supplier ID {SelectedItem.Id}, Name {SelectedItem.Name}?",
                "Delete", UniversalMessageBox.MessageBoxButton.YesNo, UniversalMessageBox.MessageBoxImage.Warning);
            return result == UniversalMessageBox.MessageBoxResult.Yes;
        }

        protected override IEnumerable<object> GetSelectedItems()
        {
            return new[] { SelectedItem };
        }

        protected override void RemoveSelectedItemFromList()
        {
            Suppliers.Remove(SelectedItem);
        }

        protected override void ClearSelectedItems()
        {
            SelectedItem = null;
        }

        protected override bool CanDelete()
        {
            return SelectedItem != null;
        }
    }
}