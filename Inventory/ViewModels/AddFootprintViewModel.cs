using System;
using Inventory.Models;
using CommunityToolkit.Mvvm.ComponentModel;

#if !WINDOWS_UWP
#endif

namespace Inventory.ViewModels
{
    public partial class AddFootprintViewModel : DbInsertViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(InsertNextCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertCloseCommand))]
        private string name;

        [ObservableProperty]
        private string comments;

        protected override void ClearFields()
        {
            Name = null;
            Comments = null;
        }

        protected override bool CanInsert()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        protected override object ConvertToEntity()
        {
            return new Footprint
            {
                FootprintName = Name.ToUpper(),
                Comments = Comments
            };
        }

        protected override bool ShouldPromptClose()
        {
            return !string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(Comments);
        }
    }
}
