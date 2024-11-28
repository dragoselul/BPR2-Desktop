using System.IO;
using System.Windows.Threading;
using BPR2_Desktop.Model;
using BPR2_Desktop.Services;
using BPR2_Desktop.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.DependencyInjection;
using Wpf.Ui;

namespace BPR2_Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private static readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(c =>
        {
            var basePath =
                Path.GetDirectoryName(AppContext.BaseDirectory)
                ?? throw new DirectoryNotFoundException(
                    "Unable to find the base directory of the application."
                );
            _ = c.SetBasePath(basePath);
        })
        .ConfigureServices(
            (context, services) =>
            {
                _ = services.AddNavigationViewPageProvider();

                // App Host
                _ = services.AddHostedService<ApplicationHostService>();

                // Theme manipulation
                _ = services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                _ = services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                _ = services.AddSingleton<INavigationService, NavigationService>();
                
                // Windows
                _ = services.AddTransient<ViewModels.MacroManagement.MacroManagementViewModel>();
                _ = services.AddTransient<MacroManagement>(serviceProvider =>
                {
                    var viewModel = serviceProvider.GetRequiredService<ViewModels.MacroManagement.MacroManagementViewModel>();
                    var navigableService = serviceProvider.GetRequiredService<INavigationService>();
                    return new MacroManagement(viewModel, navigableService);
                });
                
                _ = services.AddTransient<ViewModels.MicroManagement.MicroManagementViewModel>();
                _ = services.AddTransient<MicroManagement>(serviceProvider =>
                {
                    var viewModel = serviceProvider.GetRequiredService<ViewModels.MicroManagement.MicroManagementViewModel>();
                    var navigableService = serviceProvider.GetRequiredService<INavigationService>();
                    return new MicroManagement(viewModel, navigableService);
                });
                
                // Main window
                _ = services.AddSingleton<ViewModels.MainWindowViewModel>(
                    serviceProvider => new ViewModels.MainWindowViewModel(serviceProvider)
                );
                _ = services.AddSingleton<MainWindow>(serviceProvider =>
                {
                    var viewModel = serviceProvider.GetRequiredService<ViewModels.MainWindowViewModel>();
                    return new MainWindow(viewModel);
                });

                // Views and ViewModels for Pages
                _ = services.AddSingleton<ViewModels.MacroManagement.HomeViewModel>();
                _ = services.AddSingleton<Views.Pages.Home>();
                _ = services.AddSingleton<ViewModels.MacroManagement.DesignerViewModel>();
                _ = services.AddSingleton<Views.Pages.MacroManagementDesigner>();
                _ = services.AddSingleton<ViewModels.MicroManagement.ShelfDesignerViewModel>();
                _ = services.AddSingleton<Views.Pages.MicroManagement.ShelfDesigner>();
                _ = services.AddSingleton<ViewModels.MicroManagement.ProductViewModel>();
                _ = services.AddSingleton<Views.Pages.MicroManagement.ProductViewer>();

                // Configuration
                _ = services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
            }
        )
        .Build();

    /// <summary>
    /// Gets services.
    /// </summary>
    public static IServiceProvider Services => _host.Services;

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();
    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();
    }

    /// <summary>
    /// Occurs when an exception is thrown by an application but not handled.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Console.WriteLine("An unhandled exception occurred: {0}", e.Exception.Message);
        e.Handled = true;
    }
}