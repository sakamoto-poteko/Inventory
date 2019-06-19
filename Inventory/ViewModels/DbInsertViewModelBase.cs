using GalaSoft.MvvmLight.Messaging;
using Inventory.Framework;
using Microsoft.EntityFrameworkCore;
#if !WINDOWS_UWP
using System.Windows.Input;
using System.Windows;
#endif

namespace Inventory.ViewModels
{
    public abstract class DbInsertViewModelBase : DbViewModelBase
    {
        protected virtual void InsertNext()
        {
            if (Insert())
            {
                ClearFields();
            }
        }
        protected abstract void ClearFields();

        protected abstract bool CanInsert();

        protected abstract object ConvertToEntity();

        protected virtual bool Insert()
        {
            var context = VanillaInventoryContext;
            var model = ConvertToEntity();

            try
            {
                context.Add(model);
                context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                string err;
                if (IsConstraintsViolation(e))
                    err = "record already existed";
                else
                    err = e.InnerException?.Message ?? e.Message;
                UniversalMessageBox.Show($"Unable to save: {err}", "Save failed", UniversalMessageBox.MessageBoxButton.OK,
                    UniversalMessageBox.MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        protected virtual void InsertClose()
        {
            if (Insert())
            {
                Messenger.Default.Send(WindowMessages.CloseWindow, MsgToken);
                ClearFields();
            }
        }
        
        public RelayCommand CommandInsertNext => new RelayCommand(InsertNext, CanInsert);
        public RelayCommand CommandInsertClose => new RelayCommand(InsertClose, CanInsert);
    }
}
