using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Inventory.ViewModels;
using Inventory.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public ViewModelLocator ViewModelLocator { get; private set; }

        private readonly List<Type> _eagerSingletonList = new List<Type>();

        protected override void OnStartup(StartupEventArgs e)
        {
            var splash = new SplashScreen("/Splash.png");
            splash.Show(true);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            foreach (var type in _eagerSingletonList)
            {
                ServiceProvider.GetService(type);
            }

            _ = ServiceProvider.GetService<InventoryContext>().Database.ExecuteSqlInterpolated($"DECLARE @nop INT");

            ViewModelLocator = (ViewModelLocator)Resources["ViewModelLocator"];
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<InventoryContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);

            services.AddSingleton<AddSupplierViewModel>();
            services.AddSingleton<AddFootprintViewModel>();
            services.AddSingleton<AddLocationViewModel>();

            // Any view model that loads other table shall be transient, in order to reload on every window
            services.AddTransient<AddProductViewModel>();

            services.AddTransient<ViewSuppliersViewModel>();
            services.AddTransient<ViewLocationsViewModel>();

            _eagerSingletonList.Add(typeof(AddSupplierViewModel));
            _eagerSingletonList.Add(typeof(AddFootprintViewModel));
            _eagerSingletonList.Add(typeof(AddLocationViewModel));
            _eagerSingletonList.Add(typeof(AddProductViewModel));
        }
    }
}
