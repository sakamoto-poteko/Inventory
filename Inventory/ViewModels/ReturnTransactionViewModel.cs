﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.ViewModels
{
    public class ReturnTransactionViewModel : TransactionBaseViewModel
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        public ReturnTransactionViewModel()
        {
            TransactionType = EnumTransactionType.Return;
        }

        protected override bool ShouldPromptClose()
        {
            return false;
        }

        protected override bool Execute()
        {
            Debug.Assert(IntQuantity != null, nameof(IntQuantity) + " != null");
            Debug.Assert(SelectedInventory != null, nameof(SelectedInventory) + " != null");
            using (var transaction = _context.Database.BeginTransaction())
            {
                var entity = new Transaction
                {
                    Comments = NullOrWhitespaceAsNull(Comments),
                    Direction = Transaction.InventoryDirection.Addition,
                    Inventory = SelectedInventory,
                    Price = null,
                    Quantity = IntQuantity.Value,
                    Supplier = null,
                    Time = TransactionTime
                };
                _context.Add(entity);

                SelectedInventory.Quantity += IntQuantity.Value;
                _context.Entry(SelectedInventory).State = EntityState.Modified;

                try
                {
                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (DbUpdateException e)
                {
                    MessageBox.Show($"An error has occured: {e.Message}\n{e.InnerException?.Message}", "Save failed", MessageBoxButton.OK,
                        MessageBoxImage.Error);
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
            return SelectedInventory != null && IntQuantity.HasValue && IntQuantity.Value > 0
                   && SelectedInventory?.Quantity >= IntQuantity;
        }
    }
}