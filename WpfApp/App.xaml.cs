using Driver;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WpfApp.Components;
using WpfApp.Profile;
using Application = System.Windows.Application;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection serviceCollection = new();
            serviceCollection.ConfigureServices();

            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        public void Application_Exit()
        {
            var icon = serviceProvider.GetRequiredService<TrayIcon>();
            icon?.Dispose();
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ProfileManager>();
            services.AddSingleton<TrayIcon>();
            services.AddSingleton<KeyboardManager>();
        }
    }
}
