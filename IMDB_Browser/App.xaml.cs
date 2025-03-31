using IMDB_Browser.Navigation;
using IMDB_Browser.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace IMDB_Browser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            // Load data into view models
            LoadData();

            // Get the main view model and set it in the navigation service
            var mainViewModel = ServiceProvider.GetRequiredService<MainViewModel>();
            var NavService = ServiceProvider.GetRequiredService<INavigationService>() as NavigationService;
            NavService.SetMainViewModel(mainViewModel);

            // Show the main window
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // TODO Configure the database context
            //services.AddDbContext<ChinookContext>(options =>
            //    options.UseSqlServer(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString));

            // Register services and view models
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<FavouritesViewModel>();
            services.AddSingleton<MediaDetailViewModel>();
            services.AddSingleton<WatchListViewModel>();
            services.AddSingleton<HomeViewModel>();
        }


        private void LoadData()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                // Get the database context and view models

                //TODO import context after running EF
                //var dbcontext = scope.ServiceProvider.GetRequiredService<ChinookContext>();
                var homeViewModel = scope.ServiceProvider.GetRequiredService<HomeViewModel>();
                var mediaDetailsViewModel = scope.ServiceProvider.GetRequiredService<MediaDetailViewModel>();
                var favouritesViewModel = scope.ServiceProvider.GetRequiredService<FavouritesViewModel>();
                var watchListViewModel = scope.ServiceProvider.GetRequiredService<WatchListViewModel>();
                

                // Load data from the database and set it in the view models
                // LINQ QUERIES GO HERE:


            }
        }
    }

}
