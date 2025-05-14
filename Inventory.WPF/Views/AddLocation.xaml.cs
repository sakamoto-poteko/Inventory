using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.Messaging;
using Inventory.ViewModels;
using Inventory.WPF.Utils;

namespace Inventory.Views
{
    /// <summary>
    /// Interaction logic for AddLocation.xaml
    /// </summary>
    public partial class AddLocation : Window
    {
        private readonly CloseWindowMessageHandler _closeWindowMessageHandler;

        public AddLocation()
        {
            InitializeComponent();
            _closeWindowMessageHandler = new CloseWindowMessageHandler(this, AddLocationViewModel.MessageToken);
        }

        private readonly Regex _positiveNumberRegex =
            new Regex((string)Application.Current.Resources["NonNegativeIntegerRegexString"], RegexOptions.Compiled);

        private void TextBoxSeriesBeginEnd_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_positiveNumberRegex.IsMatch(e.Text);
        }

        private void TextBoxSeriesBeginEnd_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!_positiveNumberRegex.IsMatch(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

    }
}
