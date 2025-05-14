using Inventory.Framework;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

namespace Inventory.ViewModels
{
    public abstract partial class DbInsertViewModelBase : DbViewModelBase
    {
        [RelayCommand(CanExecute = nameof(CanInsert))]
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

        [RelayCommand(CanExecute = nameof(CanInsert))]
        protected virtual void InsertClose()
        {
            if (Insert())
            {
                WeakReferenceMessenger.Default.Send(new WindowMessage { MessageToken = MsgToken, MessageType = WindowMessage.Type.CloseWindow });
                ClearFields();
            }
        }
    }
}
