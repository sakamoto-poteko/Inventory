using System;
using System.Diagnostics;
using Inventory.Framework;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
#if !WINDOWS_UWP
using System.Windows.Input;
using System.Windows;
#endif

namespace Inventory.ViewModels
{
    public class DeductionTransaction : TransactionBaseViewModel
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        public DeductionTransaction()
        {
            TransactionType = EnumTransactionType.Retrieve;
        }

        public string Title => IsShrinkage ? "Shrinkage Inventory" : "Retrieve Inventory";

        public bool IsShrinkage
        {
            get => TransactionType == EnumTransactionType.Shrinkage;
            set
            {
                TransactionType = value ? EnumTransactionType.Shrinkage : EnumTransactionType.Retrieve;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Title));
            }
        }

        protected override bool ShouldPromptClose()
        {
            return false;
        }

        protected override bool Execute()
        {
            if (SelectedInventory.Quantity < IntQuantity)
                throw new InvalidOperationException("Not enough inventory");
            Debug.Assert(SelectedInventory != null, nameof(SelectedInventory) + " != null");

            Transaction.InventoryDirection dir;
            switch (TransactionType)
            {
                case EnumTransactionType.Retrieve:
                    dir = Transaction.InventoryDirection.Removal;
                    break;
                case EnumTransactionType.Shrinkage:
                    dir = Transaction.InventoryDirection.Shrinkage;
                    break;
                default:
                    throw new InvalidOperationException(
                        $"TransactionType {TransactionType} is not suitable for this view model");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                var entity = new Transaction
                {
                    Comments = NullOrWhitespaceAsNull(Comments),
                    Direction = dir,
                    Inventory = SelectedInventory,
                    Price = null,
                    Quantity = IntQuantity.Value,
                    Supplier = null,
                    Time = TransactionTime
                };
                _context.Add(entity);

                SelectedInventory.Quantity -= IntQuantity.Value;
                _context.Entry(SelectedInventory).State = EntityState.Modified;

                try
                {
                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (DbUpdateException e)
                {
                    UniversalMessageBox.Show($"An error has occured: {e.Message}\n{e.InnerException?.Message}",
                        "Save failed",
                        UniversalMessageBox.MessageBoxButton.OK,
                        UniversalMessageBox.MessageBoxImage.Error);
                    transaction.Rollback();
                    UndoingChangesDbContextLevel(_context);
                    return false;
                }
            }
        }

        protected override bool CheckData()
        {
            return true;
        }

        protected override bool CanInsert()
        {
            if (SelectedInventory == null)
                return false;

            if (!IntQuantity.HasValue || IntQuantity == 0)
                return false;

            if (SelectedInventory.Quantity < IntQuantity.Value)
                return false;

            return true;
        }
    }
}
