using System;
using System.Collections.ObjectModel;
using System.Linq;
using Inventory.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.ViewModels
{
    public partial class AddProductViewModel : DbInsertViewModelBase
    {
        public AddProductViewModel()
        {
            PopulateFootprints();
        }

        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NewFootprintCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertNextCommand))]
        private string productName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(InsertCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertNextCommand))]
        private string manufacturer;

        [ObservableProperty]
        private string comments;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NewFootprintCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertNextCommand))]
        private bool haveFootprint = true;

        [ObservableProperty]
        private ObservableCollection<Footprint> footprints;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(InsertCloseCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertNextCommand))]
        private int selectedFootprintId = -1;

        private void PopulateFootprints()
        {
            var oldId = SelectedFootprintId;
            var footprints = VanillaInventoryContext.Footprints.OrderBy(f => f.FootprintName).ToList();
            Footprints = new ObservableCollection<Footprint>(footprints);
            SelectedFootprintId = oldId;
        }

        [RelayCommand]
        private void NewFootprint()
        {
            var window = new Views.AddFootprint();
            window.ShowDialog();
            PopulateFootprints();
        }

        protected override void ClearFields()
        {
            ProductName = null;
            Manufacturer = null;
            Comments = null;
        }

        protected override bool CanInsert()
        {
            if (HaveFootprint && SelectedFootprintId == -1 && !string.IsNullOrWhiteSpace(Manufacturer))
                return false;
            return !string.IsNullOrWhiteSpace(ProductName);
        }

        protected override object ConvertToEntity()
        {
            return new Product
            {
                Comments = Comments,
                Manufacturer = string.IsNullOrWhiteSpace(Manufacturer) ? null : Manufacturer,
                ProductName = ProductName,
                FootprintId = HaveFootprint ? (int?)SelectedFootprintId : null
            };
        }

        protected override bool ShouldPromptClose()
        {
            return !string.IsNullOrWhiteSpace(ProductName) || !string.IsNullOrWhiteSpace(Manufacturer) ||
                   !string.IsNullOrWhiteSpace(Comments);
        }
    }
}
