using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inventory.Framework;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
#if !WINDOWS_UWP
using System.Windows.Input;
using System.Windows;
#endif

namespace Inventory.ViewModels
{
    public class ViewProductsViewModel : DbQueryViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        private string _productNameKeyword;
        private bool _productNameKeywordEnabled = true;
        private string _productCommentsKeyword;
        private bool _productCommentsKeywordEnabled;
        private string _productManufacturerKeyword;
        private bool _productManufacturerKeywordEnabled;
        private string _productFootprintKeyword;
        private bool _productFootprintKeywordEnabled;
        public override Guid MsgToken => MessageToken;

        public string ProductNameKeyword
        {
            get => _productNameKeyword;
            set
            {
                _productNameKeyword = value;
                RaisePropertyChanged();
            }
        }

        public bool ProductNameKeywordEnabled
        {
            get => _productNameKeywordEnabled;
            set
            {
                _productNameKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string ProductCommentsKeyword
        {
            get => _productCommentsKeyword;
            set
            {
                _productCommentsKeyword = value;
                RaisePropertyChanged();
            }
        }

        public bool ProductCommentsKeywordEnabled
        {
            get => _productCommentsKeywordEnabled;
            set
            {
                _productCommentsKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string ProductManufacturerKeyword
        {
            get => _productManufacturerKeyword;
            set
            {
                _productManufacturerKeyword = value;
                RaisePropertyChanged();
            }
        }

        public bool ProductManufacturerKeywordEnabled
        {
            get => _productManufacturerKeywordEnabled;
            set
            {
                _productManufacturerKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string ProductFootprintKeyword
        {
            get => _productFootprintKeyword;
            set
            {
                _productFootprintKeyword = value.ToUpper();
                RaisePropertyChanged();
            }
        }

        public bool ProductFootprintKeywordEnabled
        {
            get => _productFootprintKeywordEnabled;
            set
            {
                _productFootprintKeywordEnabled = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                RaisePropertyChanged();
            }
        }

        private Product _selectedProduct;

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                RaisePropertyChanged();
            }
        }


        protected override bool ShouldPromptClose()
        {
            return false;
        }

        protected override void SearchKeyword()
        {
            var context = VanillaInventoryContext;
            IQueryable<Product> products = context.Products.Include(p => p.Footprint);

            if (ProductNameKeywordEnabled && !string.IsNullOrWhiteSpace(ProductNameKeyword))
                products = products.Where(p => EF.Functions.Like(p.ProductName, WildcardToLike(ProductNameKeyword)));
            if (ProductManufacturerKeywordEnabled && !string.IsNullOrWhiteSpace(ProductManufacturerKeyword))
                products = products.Where(p =>
                    EF.Functions.Like(p.Manufacturer, WildcardToLike(ProductManufacturerKeyword)));
            if (ProductCommentsKeywordEnabled && !string.IsNullOrWhiteSpace(ProductCommentsKeyword))
                products = products.Where(p => EF.Functions.Like(p.Comments, WildcardToLike(ProductCommentsKeyword)));
            if (ProductFootprintKeywordEnabled && !string.IsNullOrWhiteSpace(ProductFootprintKeyword))
                products = products.Where(p =>
                    EF.Functions.Like(p.Footprint.FootprintName, WildcardToLike(ProductFootprintKeyword)));

            Products = new ObservableCollection<Product>(products);
        }

        protected override bool PromptDelete()
        {
            var result = UniversalMessageBox.Show(
                $@"Are you sure to delete product {SelectedProduct.ProductName} with footprint {SelectedProduct.Footprint?.FootprintName ?? "<empty>"} by {SelectedProduct.Manufacturer ?? "unknown manufacturer"}?",
                "Delete", UniversalMessageBox.MessageBoxButton.YesNo, UniversalMessageBox.MessageBoxImage.Warning);
            return result == UniversalMessageBox.MessageBoxResult.Yes;
        }

        protected override IEnumerable<object> GetSelectedItems()
        {
            return new[] { SelectedProduct };
        }

        protected override void RemoveSelectedItemFromList()
        {
            Products.Remove(SelectedProduct);
        }

        protected override void ClearSelectedItems()
        {
            SelectedProduct = null;
        }

        protected override bool CanDelete()
        {
            return SelectedProduct != null;
        }
    }
}
