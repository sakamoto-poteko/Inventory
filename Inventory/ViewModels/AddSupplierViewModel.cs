using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.ViewModels
{
    public class AddSupplierViewModel : ViewModelBase
    {
        private readonly InventoryContext _context;
        private string _name;
        private string _comments;

        public AddSupplierViewModel(InventoryContext context)
        {
            _context = context;
        }

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

        private bool InsertSupplier()
        {
            var supplier = new Supplier
            {
                Comments = Comments,
                Name = Name
            };
            _context.Add(supplier);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                MessageBox.Show($"Unable to insert: {e.Message}", "Insertion failed", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            return false;
        }

        private void CreateCommands()
        {
            CommandInsertNext = new RelayCommand(() =>
            {
                var inserted = InsertSupplier();
                if (inserted)
                {
                    Name = string.Empty;
                    Comments = string.Empty;
                }
            }, () => !string.IsNullOrWhiteSpace(Name));

            CommandInsertClose = 
        }

        public RelayCommand CommandInsertNext { get; private set; }

        public RelayCommand CommandInsertClose { get; private set; }
        public ICommand CommandClose { get; }

    }
}
