using System;
using System.Data.SqlClient;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Inventory.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
#if WINDOWS_UWP
using Windows.UI.Xaml;
#else
using System.Windows.Input;
using System.Windows;
#endif

namespace Inventory.ViewModels
{
    public abstract class DbViewModelBase : ViewModelBase
    {
        public abstract Guid MsgToken { get; }

        /// <summary>
        /// Get a new ef InventoryContext
        /// </summary>
        protected InventoryContext VanillaInventoryContext => Globals.Instance.ServiceProvider.GetService<InventoryContext>();

        protected bool IsConstraintsViolation(DbUpdateException e)
        {
            if (e.InnerException is SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 547 || ex.Number == 2601)
                    return true;
            }

            return false;
        }

        protected string WildcardToLike(string like)
        {
            return like.Replace('?', '_').Replace('*', '%');
        }

        protected abstract bool ShouldPromptClose();

        private void Close()
        {
            if (ShouldPromptClose())
            {
                var result = UniversalMessageBox.Show("Close without saving changes?", "Close", UniversalMessageBox.MessageBoxButton.YesNo,
                    UniversalMessageBox.MessageBoxImage.Exclamation);
                if (result == UniversalMessageBox.MessageBoxResult.No)
                    return;
            }
            Messenger.Default.Send(WindowMessages.CloseWindow, MsgToken);
        }

        protected string NullOrWhitespaceAsNull(string val)
        {
            return string.IsNullOrWhiteSpace(val) ? null : val;
        }

        protected static void UndoingChangesDbContextLevel(DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        entry.Reload();
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public RelayCommand CommandClose => new RelayCommand(Close);
    }
}
