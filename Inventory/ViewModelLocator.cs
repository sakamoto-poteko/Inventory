using System;
using System.Windows;
using Inventory.Models;
using Inventory.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory
{
    public class ViewModelLocator
    {
        private readonly IServiceProvider _serviceProvider;
        public ViewModelLocator()
        {
            _serviceProvider = ((App)Application.Current).ServiceProvider;
        }

        public AddSupplierViewModel AddSupplierViewModel => _serviceProvider.GetService<AddSupplierViewModel>();
        public AddFootprintViewModel AddFootprintViewModel => _serviceProvider.GetService<AddFootprintViewModel>();
        public AddLocationViewModel AddLocationViewModel => _serviceProvider.GetService<AddLocationViewModel>();
        public AddProductViewModel AddProductViewModel => _serviceProvider.GetService<AddProductViewModel>();

        public ViewSuppliersViewModel ViewSuppliersViewModel => _serviceProvider.GetService<ViewSuppliersViewModel>();
        public ViewLocationsViewModel ViewLocationsViewModel => _serviceProvider.GetService<ViewLocationsViewModel>();
        public ViewFootprintsViewModel ViewFootprintsViewModel => _serviceProvider.GetService<ViewFootprintsViewModel>();
        public ViewProductsViewModel ViewProductsViewModel => _serviceProvider.GetService<ViewProductsViewModel>();

        public PurchaseTransactionViewModel PurchaseTransactionViewModel => _serviceProvider.GetService<PurchaseTransactionViewModel>();
        public ReturnTransactionViewModel ReturnTransactionViewModel => _serviceProvider.GetService<ReturnTransactionViewModel>();
        public DeductionTransactionBase DeductionTransactionViewModel => _serviceProvider.GetService<DeductionTransactionBase>();
    }
}
