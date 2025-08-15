using MaiMvvmFramework;
using MaiMvvmFramework.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ps1ToExe.ViewModels;
using Ps1ToExe.Views;
using System.Windows;

namespace Ps1ToExe;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : MaiMvvmApplication
{
    private bool _disposed;

    public App()
    {
        AppHost = new HostBuilder()
            // Implement other stuffs
            .ConfigureLogging()
            .ConfigureServices(ConfigureServices)
            .ConfigureMvvmUi<MainWindow, MainWindowViewModel>("Ps1ToExe")
            .Build();
        InitializeComponent();
    }

    /// <summary>
    /// Disposes this instance.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            AppHost.Dispose();
        }

        _disposed = true;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
    protected override void OnStartup(StartupEventArgs e)
    {
        StartApplication(e);
        //ShowSplashView();

        //Start the main window in a new thread.
        _ = Task.Factory.StartNew(() =>
        {
            Dispatcher.Invoke(() =>
            {
                try
                {
                    ShowMainWindow();
                    //HideSplashView();
                }
                catch (Exception exception)
                {
                    Logger?.Fatal(exception, "Fatal initializing application: {Message}",
                       exception.GetBaseException().Message);
                    Current.Shutdown();
                }
            });
        });
    }

    /// <summary>
    /// Configures the services.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>IServiceProvider.</returns>
    private void ConfigureServices(HostBuilderContext builder, IServiceCollection serviceCollection)
    {
        // TODO: Consolidate this into a factory method in MaiMvvmApplication
        //serviceCollection.AddTransient<ISettingsFlyoutContentView, SettingsView>();
        //serviceCollection.AddTransient<ISettingsFlyoutViewModel, SettingsViewModel>();
    }
}

