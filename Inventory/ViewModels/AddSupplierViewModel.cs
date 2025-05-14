using System;
using Inventory.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.ViewModels
{
    public partial class AddSupplierViewModel : DbInsertViewModelBase
    {
        public static readonly Guid MessageToken = Guid.NewGuid();


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(InsertNextCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertCloseCommand))]
        private string name;

        [ObservableProperty]
        private string comments;

        protected override object ConvertToEntity()
        {
            return new Supplier
            {
                Name = Name,
                Comments = Comments
            };
        }

        protected override void ClearFields()
        {
            Name = null;
            Comments = null;
        }

        protected override bool ShouldPromptClose()
        {
            return !string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(Comments);
        }

        protected override bool CanInsert()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        public override Guid MsgToken => MessageToken;
    }
}
