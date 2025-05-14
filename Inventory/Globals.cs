using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

#if WINDOWS_UWP
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace Inventory
{
    public class Globals
    {
        private Globals() { }
        private static Globals _instance;
        private Regex _priceRegex;
        private Regex _positiveIntegerRegex;
        public static Globals Instance => _instance ?? (_instance = new Globals());

        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; set; }
        public ViewModelLocator ViewModelLocator { get; set; }

        public Regex PriceRegex => _priceRegex ?? (_priceRegex = new Regex(
                                       (string)Application.Current.Resources["PriceRegexString"],
                                       RegexOptions.Compiled));

        public Regex PositiveIntegerRegex => _positiveIntegerRegex ?? (_positiveIntegerRegex = new Regex(
                                                 (string)Application.Current.Resources["NonNegativeIntegerRegexString"], 
                                                 RegexOptions.Compiled));
    }
}
