using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inventory.Framework;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;

#if !WINDOWS_UWP
#endif

namespace Inventory.ViewModels
{
    public partial class ViewProductsViewModel : DbQueryViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        [ObservableProperty]
        private string productNameKeyword;

        [ObservableProperty]
        private bool productNameKeywordEnabled = true;

        [ObservableProperty]
        private string productCommentsKeyword;

        [ObservableProperty]
        private bool productCommentsKeywordEnabled;

        [ObservableProperty]
        private string productManufacturerKeyword;

        [ObservableProperty]
        private bool productManufacturerKeywordEnabled;

        [ObservableProperty]
        private string productFootprintKeyword;

        [ObservableProperty]
        private bool productFootprintKeywordEnabled;
        
        [ObservableProperty]
        private ObservableCollection<Product> products;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private Product selectedProduct;


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
