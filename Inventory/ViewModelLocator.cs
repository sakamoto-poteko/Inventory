﻿using System;
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
            _serviceProvider = ((App) Application.Current).ServiceProvider;
        }

        public AddSupplierViewModel AddSupplierViewModel => _serviceProvider.GetService<AddSupplierViewModel>();
        public AddFootprintViewModel AddFootprintViewModel => _serviceProvider.GetService<AddFootprintViewModel>();
        public AddLocationViewModel AddLocationViewModel => _serviceProvider.GetService<AddLocationViewModel>();
        public AddProductViewModel AddProductViewModel => _serviceProvider.GetService<AddProductViewModel>();
    }
}