using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.ViewModels
{
    public class AddLocationViewModel : DbInsertViewModelBase
    {
        public static Guid MessageToken = Guid.NewGuid();
        public override Guid MsgToken => MessageToken;

        private string _locationName;

        public string LocationName
        {
            get => _locationName;
            set
            {
                _locationName = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private string _locationUnit;

        public string LocationUnit
        {
            get => _locationUnit;
            set
            {
                _locationUnit = value;
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

        private bool _isSeries;

        public bool IsSeries
        {
            get => _isSeries;
            set
            {
                _isSeries = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private int _seriesBegin;

        public string SeriesBegin
        {
            get => _seriesBegin.ToString();
            set
            {
                if (!int.TryParse(value, out _seriesBegin))
                    _seriesBegin = 0;

                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private int _seriesEnd;

        public string SeriesEnd
        {
            get => _seriesEnd.ToString();
            set
            {
                if (!int.TryParse(value, out _seriesEnd))
                    _seriesEnd = 0;
                RaisePropertyChanged();
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
                MessageBox.Show($"Unable to insert: {err}", "Insertion failed", MessageBoxButton.OK,
                    MessageBoxImage.Error);
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
