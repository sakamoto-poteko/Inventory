using System.Collections.Generic;
using System.Windows;
using Inventory.Framework;
using Microsoft.EntityFrameworkCore;

namespace Inventory.ViewModels
{
    public abstract class DbQueryViewModelBase : DbViewModelBase
    {
        public RelayCommand CommandSearch => new RelayCommand(SearchKeyword);
        public RelayCommand CommandDelete => new RelayCommand(Delete, CanDelete);
        protected abstract void SearchKeyword();
        protected abstract bool PromptDelete();
        protected abstract IEnumerable<object> GetSelectedItems();
        protected abstract void RemoveSelectedItemFromList();
        protected abstract void ClearSelectedItems();

        protected bool DeleteSelectedItem()
        {
            var selectedItem = GetSelectedItems();
            if (selectedItem != null)
            {
                var context = VanillaInventoryContext;
                context.RemoveRange(selectedItem);

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    string err;
                    if (IsUniqueRowViolation(e))
                        err = "errurv";
                    else
                        err = e.InnerException?.Message ?? e.Message;
                    MessageBox.Show($"Unable to insert: {err}", "Insertion failed", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }

                RemoveSelectedItemFromList();
                ClearSelectedItems();
            }

            return true;
        }

        protected virtual void Delete()
        {
            if (PromptDelete())
                DeleteSelectedItem();
        }

        protected abstract bool CanDelete();
    }
}