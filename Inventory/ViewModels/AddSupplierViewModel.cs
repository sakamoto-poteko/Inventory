using System;
using System.Windows;
using System.Windows.Input;
using Inventory.Models;

namespace Inventory.ViewModels
{
    public class AddSupplierViewModel : DbInsertViewModelBase
    {
        public static readonly Guid MessageToken = Guid.NewGuid();

        private string _name;
        private string _comments;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                RaisePropertyChanged();
            }
        }

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
