using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Inventory.Framework;
using Inventory.Models;

namespace Inventory.ViewModels
{
    public class AddProductViewModel : DbInsertViewModelBase
    {
        public AddProductViewModel()
        {
            PopulateFootprints();
        }

        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        private string _productName;

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _manufacturer;

        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                _manufacturer = value;
                RaisePropertyChanged();
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

        public ObservableCollection<Footprint> Footprints
        {
            get => _footprints;
            private set
            {
                _footprints = value;
                RaisePropertyChanged();
            }
        }

        public int SelectedFootprintId
        {
            get => _selectedFootprintId;
            set
            {
                _selectedFootprintId = value;
                RaisePropertyChanged();
            }
        }

        private void PopulateFootprints()
        {
            var oldId = SelectedFootprintId;
            var footprints = VanillaInventoryContext.Footprints.OrderBy(f => f.FootprintName).ToList();
            Footprints = new ObservableCollection<Footprint>(footprints);
            SelectedFootprintId = oldId;
        }

        private void NewFootprint()
        {
            var window = new Views.AddFootprint();
            window.ShowDialog();
            PopulateFootprints();
        }

        private bool _haveFootprint = true;
        private ObservableCollection<Footprint> _footprints;
        private int _selectedFootprintId = -1;

        public bool HaveFootprint
        {
            get => _haveFootprint;
            set
            {
                _haveFootprint = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public RelayCommand CommandNewFootprint => new RelayCommand(NewFootprint);

        protected override void ClearFields()
        {
            ProductName = null;
            Manufacturer = null;
            Comments = null;
        }

        protected override bool CanInsert()
        {
            if (HaveFootprint && SelectedFootprintId == -1)
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
