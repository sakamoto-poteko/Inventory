using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Inventory.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.ViewModels
{
    public abstract class DbViewModelBase : ViewModelBase
    {
        public abstract Guid MsgToken { get; }

        /// <summary>
        /// Get a new ef InventoryContext
        /// </summary>
        protected InventoryContext VanillaInventoryContext => ((App) Application.Current).ServiceProvider.GetService<InventoryContext>();

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
                var result = MessageBox.Show("Close without saving changes?", "Close", MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.No)
                    return;
            }
            Messenger.Default.Send(WindowMessages.CloseWindow, MsgToken);
        }

        public RelayCommand CommandClose => new RelayCommand(Close);
    }
}
