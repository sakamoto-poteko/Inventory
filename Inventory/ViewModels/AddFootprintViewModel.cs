using System;
using Inventory.Models;
#if !WINDOWS_UWP
using System.Windows.Input;
using System.Windows;
#endif

namespace Inventory.ViewModels
{
    public class AddFootprintViewModel : DbInsertViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value?.ToUpper();
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _comments;

        public string Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                RaisePropertyChanged();
            }
        }


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
                FootprintName = Name,
                Comments = Comments
            };
        }

        protected override bool ShouldPromptClose()
        {
            return !string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(Comments);
        }
    }
}
