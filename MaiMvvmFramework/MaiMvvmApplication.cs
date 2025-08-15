using MahApps.Metro.Controls;
using MaiMvvmFramework.ViewModels;
using MaiMvvmFramework.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;
using System.Windows;
using MaiMvvmFramework.Common;
using MaiMvvmFramework.Hosting;

namespace MaiMvvmFramework;

/// <summary>
/// Represents the main WPF application class for the MaiMvvmFramework.
/// Manages application startup, dependency injection, splash and main window display, and logging.
/// </summary>
public partial class MaiMvvmApplication : Application
{
    private MetroWindow _splashView = null!;

    /// <summary>
    /// Gets or sets the <see cref="IHost"/> instance for the application.
    /// All configurable items and services are contained here during startup.
    /// </summary>
    public static IHost AppHost { get; protected set; } = null!;

    /// <summary>
    /// Gets the name of the application from the executing assembly.
    /// </summary>
    /// <value>
    /// The name of the application, or "Application name not found" if unavailable.
    /// </value>
    public static string ApplicationName => Assembly.GetExecutingAssembly().FullName ?? "Application name not found";

    /// <summary>
    /// Gets the Serilog <see cref="ILogger"/> instance from the application's service provider, if available.
    /// Returns <c>null</c> if the logger service is not registered.
    /// </summary>
    /// <value>
    /// The <see cref="ILogger"/> instance for logging, or <c>null</c> if unavailable.
    /// </value>
    public static ILogger? Logger => Services.TryGetService(out ILogger logger) ? logger : null;

    /// <summary>
    /// Gets the <see cref="IApplicationConfiguration"/> instance from the application's service provider, if available.
    /// Returns <c>null</c> if the configuration service is not registered.
    /// </summary>
    /// <value>
    /// The <see cref="IApplicationConfiguration"/> instance for application settings, or <c>null</c> if unavailable.
    /// </value>
    public static IApplicationConfiguration? Configuration => Services.TryGetService(out IApplicationConfiguration configuration) ? configuration : null;

    /// <summary>
    /// Gets the root <see cref="IServiceProvider"/> for dependency resolution.
    /// </summary>
    /// <value>
    /// The application's service provider.
    /// </value>
    public static IServiceProvider Services => AppHost.Services;

    /// <summary>
    /// Gets the <see cref="IShellContentViewModel"/> instance from the application's service provider.
    /// </summary>
    /// <value>
    /// The main shell content view model.
    /// </value>
    public static IShellContentViewModel ShellContentViewModel => Services.GetRequiredService<IShellContentViewModel>();

    /// <summary>
    /// Closes the splash view window if it is open.
    /// </summary>
    public void HideSplashView()
    {
        _splashView.Close();
    }

    /// <summary>
    /// Displays the main application window, which implements <see cref="IShellView"/>.
    /// </summary>
    public void ShowMainWindow()
    {
        MainWindow = (MetroWindow)Services.GetRequiredService<IShellView>();
        MainWindow.Show();
    }

    /// <summary>
    /// Displays the splash view window, which implements <see cref="ISplashView"/>.
    /// Sets the splash view as the application's main window.
    /// </summary>
    public void ShowSplashView()
    {
        _splashView = (MetroWindow)Services.GetRequiredService<ISplashView>();
        MainWindow = _splashView;
        _splashView.Show();
    }

    /// <summary>
    /// Starts the application lifecycle and triggers host startup asynchronously.
    /// Calls <see cref="Application.OnStartup(StartupEventArgs)"/> after starting the host.
    /// </summary>
    /// <param name="e">The startup event arguments.</param>
    public void StartApplication(StartupEventArgs e)
    {
        Task.Run(async () => await AppHost.StartAsync());
        base.OnStartup(e);
    }
}