using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
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
        protected InventoryContext InventoryContext => ((App) Application.Current).ServiceProvider.GetService<InventoryContext>();

        protected bool IsUniqueRowViolation(DbUpdateException e)
        {
            if (e.InnerException is SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 547 || ex.Number == 2601)
                    return true;
            }

            return false;
        }
    }
}
