using System;
using Inventory.Framework;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Inventory.ViewModels
{
    public partial class AddLocationViewModel : DbInsertViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(InsertNextCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertCloseCommand))]
        private string locationName;

        [ObservableProperty]
        private string locationUnit;

        [ObservableProperty]
        private string comments;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(InsertNextCommand))]
        [NotifyCanExecuteChangedFor(nameof(InsertCloseCommand))]
        private bool isSeries;

        private int _seriesBegin;

        public string SeriesBegin
        {
            get => _seriesBegin.ToString();
            set
            {
                if (!int.TryParse(value, out var seriesBegin))
                    SetProperty(ref _seriesBegin, 0);
                else
                    SetProperty(ref _seriesBegin, seriesBegin);

                CommandManager.InvalidateRequerySuggested();
            }
        }

        private int _seriesEnd;

        public string SeriesEnd
        {
            get => _seriesEnd.ToString();
            set
            {
                if (!int.TryParse(value, out var seriesEnd))
                    SetProperty(ref _seriesEnd, 0);
                else
                    SetProperty(ref _seriesEnd, seriesEnd);

                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool IsLocationUnitValid()
        {
            if (IsSeries)
            {
                return _seriesBegin < _seriesEnd && _seriesBegin >= 0;
            }
            else
            {
                // Unit can be null
                return true;
            }
        }

        protected override bool Insert()
        {
            var context = VanillaInventoryContext;
            if (IsSeries)
            {
                for (var i = _seriesBegin; i <= _seriesEnd; ++i)
                {
                    context.Add(new Location
                    {
                        LocationName = LocationName,
                        LocationUnit = i.ToString(),
                        Comments = Comments
                    });
                }
            }
            else
            {
                context.Add(new Location
                {
                    LocationName = LocationName,
                    LocationUnit = string.IsNullOrWhiteSpace(LocationUnit) ? null : LocationUnit,
                    Comments = Comments
                });
            }

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                string err;
                if (IsConstraintsViolation(e))
                    err = "record already existed";
                else
                    err = e.InnerException?.Message ?? e.Message;
                UniversalMessageBox.Show($"Unable to insert: {err}",
                    "Insertion failed",
                    UniversalMessageBox.MessageBoxButton.OK,
                    UniversalMessageBox.MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        protected override void ClearFields()
        {
            LocationName = null;
            LocationUnit = null;
            Comments = null;
        }

        protected override bool CanInsert()
        {
            return IsLocationUnitValid() && !string.IsNullOrEmpty(LocationName);
        }

        protected override object ConvertToEntity()
        {
            throw new NotImplementedException();
        }

        protected override bool ShouldPromptClose()
        {
            if (IsSeries)
            {
                return !string.IsNullOrWhiteSpace(LocationName) || !string.IsNullOrWhiteSpace(LocationUnit);
            }
            else
            {
                return !string.IsNullOrWhiteSpace(LocationName);
            }
        }
    }
}
