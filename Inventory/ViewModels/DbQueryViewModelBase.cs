using System.Collections.Generic;
using Inventory.Framework;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.Input;

#if !WINDOWS_UWP
#endif

namespace Inventory.ViewModels
{
    public abstract partial class DbQueryViewModelBase : DbViewModelBase
    {
        [RelayCommand]
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
                    if (IsConstraintsViolation(e))
                        err = "used by other records";
                    else
                        err = e.InnerException?.Message ?? e.Message;
                    UniversalMessageBox.Show($"Unable to save: {err}",
                        "Save failed",
                        UniversalMessageBox.MessageBoxButton.OK,
                        UniversalMessageBox.MessageBoxImage.Error);
                    return false;
                }

                RemoveSelectedItemFromList();
                ClearSelectedItems();
            }

            return true;
        }

        [RelayCommand(CanExecute = nameof(CanDelete))]
        protected virtual void Delete()
        {
            if (PromptDelete())
                DeleteSelectedItem();
        }

        protected abstract bool CanDelete();
    }
}