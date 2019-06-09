using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.ViewModels
{
    public class ViewSuppliersViewModel : DbQueryViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();

        private string _commentsKeyword;
        private bool _commentsKeywordEnabled;
        private string _nameKeyword;
        private bool _nameKeywordEnabled = true;

        private Supplier _selectedItem;
        private ObservableCollection<Supplier> _suppliers;

        public string NameKeyword
        {
            get => _nameKeyword;
            set
            {
                _nameKeyword = value;
                RaisePropertyChanged();
            }
        }

        public bool NameKeywordEnabled
        {
            get => _nameKeywordEnabled;
            set
            {
                _nameKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string CommentsKeyword
        {
            get => _commentsKeyword;
            set
            {
                _commentsKeyword = value;
                RaisePropertyChanged();
            }
        }

        public bool CommentsKeywordEnabled
        {
            get => _commentsKeywordEnabled;
            set
            {
                _commentsKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Supplier> Suppliers
        {
            get => _suppliers;
            set
            {
                _suppliers = value;
                RaisePropertyChanged();
            }
        }

        public Supplier SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

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
            var result = MessageBox.Show(
                $"Are you sure to delete supplier ID {SelectedItem.Id}, Name {SelectedItem.Name}?",
                "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes;
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