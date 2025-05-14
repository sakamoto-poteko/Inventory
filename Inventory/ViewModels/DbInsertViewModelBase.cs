using Inventory.Framework;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;

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
                WeakReferenceMessenger.Default.Send(new WindowMessage { MessageToken = MsgToken, MessageType = WindowMessage.Type.CloseWindow });
                ClearFields();
            }
        }

        public RelayCommand CommandInsertNext => new RelayCommand(InsertNext, CanInsert);
        public RelayCommand CommandInsertClose => new RelayCommand(InsertClose, CanInsert);
    }
}
