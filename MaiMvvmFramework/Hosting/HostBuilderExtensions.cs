using MahApps.Metro.Controls.Dialogs;
using MaiMvvmFramework.Common;
using MaiMvvmFramework.Data;
using MaiMvvmFramework.ViewModels;
using MaiMvvmFramework.ViewModels.Dialogs;
using MaiMvvmFramework.Views;
using MaiMvvmFramework.Views.Dialogs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Runtime;
using MaiMvvmFramework.ViewModels.Flyouts;
using MaiMvvmFramework.Views.Flyouts;
using ShellView = MaiMvvmFramework.Views.ShellView;

namespace MaiMvvmFramework.Hosting;

/// <summary>
/// Provides extension methods for configuring MVVM UI, dialogs, logging, application settings, and SQL management
/// in a WPF application using the .NET Generic Host infrastructure.
/// </summary>
/// <remarks>
/// This static class is intended to be used in the application's host builder setup to register and configure
/// core services required for MVVM, dialog coordination, logging, configuration management, and data access.
/// </remarks>
public static class HostBuilderExtensions
{
    /// <summary>
    /// Registers MVVM UI services for the application.
    /// Adds the shell view, the shell view model, shell content view, and shell content view model as singletons.
    /// Also registers the MahApps.Metro DialogCoordinator instance for dialog coordination.
    /// </summary>
    /// <typeparam name="TV">Shell content view type implementing <see cref="IShellContentView"/>.</typeparam>
    /// <typeparam name="TVm">Shell content view model type implementing <see cref="IShellContentViewModel"/>.</typeparam>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <param name="applicationName">The name of the application.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    public static IHostBuilder ConfigureMvvmUi<TV, TVm>(this IHostBuilder hostBuilder, string applicationName)
        where TV : class, IShellContentView
        where TVm : class, IShellContentViewModel
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices((_, services) =>
        {
            if (services.Any(sd => sd.ServiceType == typeof(IShellView))
                || services.Any(sd => sd.ServiceType == typeof(IShellViewModel))
                || services.Any(sd => sd.ServiceType == typeof(IShellContentView))
                || services.Any(sd => sd.ServiceType == typeof(IShellContentViewModel))
                || services.Any(sd => sd.ServiceType == typeof(IDialogCoordinator))
                || services.Any(sd => sd.ServiceType == typeof(ISplashView))
                || services.Any(sd => sd.ServiceType == typeof(ISplashViewModel)))
            {
                throw new AmbiguousImplementationException(Resources.UiAlreadyInitialzed);
            }

            services.AddKeyedSingleton("ApplicationName", applicationName);
            services.AddSingleton<IShellView, ShellView>();
            services.AddSingleton<IShellViewModel, ShellViewModel>();
            services.AddSingleton<IShellContentView, TV>();
            services.AddSingleton<IShellContentViewModel, TVm>();
            services.AddSingleton(DialogCoordinator.Instance);
            services.AddSingleton<ISplashView, SplashView>();
            services.AddSingleton<ISplashViewModel>(_ =>
            {
                ApplicationInfo.ProductName = applicationName;
                return new SplashViewModel(Resources.LoadingData);
            });
        });

        return hostBuilder;
    }

    /// <summary>
    /// Registers a dialog content view and its view model as transient services.
    /// Throws <see cref="AmbiguousImplementationException"/> if either type is already registered in the service collection.
    /// </summary>
    /// <typeparam name="TV">Dialog content view type implementing <see cref="IDialogContentView"/>.</typeparam>
    /// <typeparam name="TVm">Dialog view model type implementing <see cref="IDialogViewModel"/>.</typeparam>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    public static IHostBuilder ConfigureDialog<TV, TVm>(this IHostBuilder hostBuilder)
        where TV : class, IDialogContentView
        where TVm : class, IDialogViewModel
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices((_, services) =>
        {
            if (services.Any(sd => sd.ServiceType == typeof(TV))
                || services.Any(sd => sd.ServiceType == typeof(TVm)))
            {
                throw new AmbiguousImplementationException(Resources.DialogAlreadyImplemented);
            }

            services.AddTransient<TV>();
            services.AddTransient<TVm>();
        });

        return hostBuilder;
    }

    /// <summary>
    /// Registers a flyout content view and its view model as transient services.
    /// Throws <see cref="AmbiguousImplementationException"/> if either type is already registered in the service collection.
    /// </summary>
    /// <typeparam name="TV">Flyout content view type implementing <see cref="IFlyoutContentView"/>.</typeparam>
    /// <typeparam name="TVm">Flyout view model type implementing <see cref="IFlyoutViewModel"/>.</typeparam>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    public static IHostBuilder ConfigureFlyout<TV, TVm>(this IHostBuilder hostBuilder)
        where TV : class, IFlyoutContentView
        where TVm : class, IFlyoutViewModel
    {
        ArgumentNullException.ThrowIfNull(hostBuilder);

        hostBuilder.ConfigureServices((_, services) =>
        {
            if (services.Any(sd => sd.ServiceType == typeof(TV))
                || services.Any(sd => sd.ServiceType == typeof(TVm)))
            {
                throw new AmbiguousImplementationException(Resources.DialogAlreadyImplemented);
            }

            services.AddTransient<TV>();
            services.AddTransient<TVm>();
        });

        return hostBuilder;
    }

    /// <summary>
    /// Configures Serilog-based logging for the application.
    /// Sets up the logger using the application's configuration and registers the logger as a singleton service.
    /// </summary>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureLogging((context, logBuilder) =>
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(context.Configuration).CreateLogger();
            logBuilder.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(context.Configuration).CreateLogger(), dispose: true);
            logBuilder.Services.AddLogging();
        });

        return hostBuilder.ConfigureServices((_, services) =>
        {
            services.AddSingleton(Log.Logger);
        });
    }

    /// <summary>
    /// Configures application settings by binding the "Application" section of the configuration to a local configuration instance.
    /// Sets the application name and registers both the local and lazy-loaded configuration instances as singletons.
    /// </summary>
    /// <typeparam name="TL">Local configuration type implementing <see cref="IAppSettings"/>.</typeparam>
    /// <typeparam name="TA">Application configuration type implementing <see cref="IApplicationConfiguration"/>.</typeparam>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    public static IHostBuilder ConfigureAppSettings<TL, TA>(this IHostBuilder hostBuilder)
        where TL : IAppSettings, new()
        where TA : IApplicationConfiguration, new()
    {
        hostBuilder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            configBuilder.AddJsonFile("appsettings.json");
        });

        return hostBuilder.ConfigureServices((context, services) =>
        {
            var configuration = new TL();
            context.Configuration.GetSection("Application").Bind(configuration, c => c.BindNonPublicProperties = true);
            services.AddSingleton(configuration.GetType(), configuration);

            var appConfiguration = new TA
            {
                AppSettings = configuration
            };
            services.AddSingleton<IApplicationConfiguration>(appConfiguration);
        });
    }

    /// <summary>
    /// Configures SQL management services by registering the connection factory and local repository as singletons.
    /// The repository is created using dependency injection for the connection factory, local configuration, and logger.
    /// Also registers the repository as <see cref="IRepository"/>.
    /// </summary>
    /// <typeparam name="TLr">Local repository type implementing <see cref="ILocalRepository"/>.</typeparam>
    /// <typeparam name="TAc">Application configuration type implementing <see cref="IApplicationConfiguration"/>.</typeparam>
    /// <typeparam name="TLc">Local configuration type implementing <see cref="IAppSettings"/>.</typeparam>
    /// <param name="hostBuilder">The host builder to configure.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    public static IHostBuilder ConfigureSqlManagement<TLr, TAc, TLc>(this IHostBuilder hostBuilder)
        where TLr : class, IRepository
        where TAc : class, IApplicationConfiguration, new()
        where TLc : class, IAppSettings
    {
        hostBuilder.ConfigureServices((_, services) =>
        {
            services.AddSingleton<IConnectionFactory>(p => new SqlConnectionFactory(p.GetRequiredService<IApplicationConfiguration>().AppSettings.ConnectionString));
            services.AddSingleton<IConfigurationManager<TAc>>(provider => new SqlConfigurationManager<TAc>(provider.GetRequiredService<IConnectionFactory>()));
            services.AddSingleton<IRepository>(provider =>
            {
                var d = ActivatorUtilities.CreateInstance<TLr>(
                    provider,
                    provider.GetRequiredService<IConnectionFactory>(),
                    provider.GetRequiredService<IApplicationConfiguration>().AppSettings,
                    provider.GetRequiredService<ILogger>()
                );
                return d;
            });
            services.AddSingleton<IRepository>(provider => provider.GetRequiredService<IRepository>());
        });

        return hostBuilder;
    }
}