using MahApps.Metro.Controls.Dialogs;
using MaiMvvmFramework.Common;
using MaiMvvmFramework.Data;
using MaiMvvmFramework.Hosting;
using MaiMvvmFramework.ViewModels;
using MaiMvvmFramework.ViewModels.Dialogs;
using MaiMvvmFramework.ViewModels.Flyouts;
using MaiMvvmFramework.Views;
using MaiMvvmFramework.Views.Dialogs;
using MaiMvvmFramework.Views.Flyouts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MaiMvvmFramework.Tests.Hosting;

public class HostBuilderExtensionsTests
{   
    [Fact]
    public void ConfigureMvvmUi_RegistersRequiredServices()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureMvvmUi<TestShellView, TestShellViewModel>("TestApp");
        var host = hostBuilder.Build();
        var services = host.Services;

        Assert.NotNull(services.GetService<IShellView>());
        Assert.NotNull(services.GetService<IShellViewModel>());
        Assert.NotNull(services.GetService<IShellContentView>());
        Assert.NotNull(services.GetService<IShellContentViewModel>());
        Assert.NotNull(services.GetService<IDialogCoordinator>());
        //Assert.NotNull(services.GetService<ISplashView>());
        //Assert.NotNull(services.GetService<ISplashViewModel>());
    }

    [Fact]
    public void ConfigureDialog_RegistersDialogServices()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureDialog<TestDialogView, TestDialogViewModel>();
        var host = hostBuilder.Build();
        var services = host.Services;

        Assert.NotNull(services.GetService<TestDialogView>());
        Assert.NotNull(services.GetService<TestDialogViewModel>());
    }

    [Fact]
    public void ConfigureFlyout_RegistersFlyoutServices()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureFlyout<TestFlyoutView, TestFlyoutViewModel>();
        var host = hostBuilder.Build();
        var services = host.Services;

        Assert.NotNull(services.GetService<TestFlyoutView>());
        Assert.NotNull(services.GetService<TestFlyoutViewModel>());
    }

    [Fact]
    public void ConfigureLogging_RegistersLogger()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureLogging();
        var host = hostBuilder.Build();
        var logger = host.Services.GetService<Serilog.ILogger>();
        Assert.NotNull(logger);
    }

    [Fact]
    public void ConfigureAppSettings_RegistersAppSettingsAndConfiguration()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureAppSettings<TestAppSettings, TestApplicationConfiguration>();
        var host = hostBuilder.Build();
        var appSettings = host.Services.GetService(typeof(TestAppSettings));
        var appConfig = host.Services.GetService<IApplicationConfiguration>();
        Assert.NotNull(appSettings);
        Assert.NotNull(appConfig);
    }

    [Fact]
    public void ConfigureSqlManagement_RegistersSqlServices()
    {
        var hostBuilder = Host.CreateDefaultBuilder();
        hostBuilder.ConfigureAppSettings<TestAppSettings, TestApplicationConfiguration>();
        hostBuilder.ConfigureSqlManagement<TestRepository, TestApplicationConfiguration, TestAppSettings>();
        var host = hostBuilder.Build();
        var connectionFactory = host.Services.GetService<IConnectionFactory>();
        var configManager = host.Services.GetService<IConfigurationManager<TestApplicationConfiguration>>();
        var repository = host.Services.GetService<IRepository>();
        Assert.NotNull(connectionFactory);
        Assert.NotNull(configManager);
        Assert.NotNull(repository);
    }

    // Dummy implementations for testing
    public class TestDialogView : IDialogContentView
    {
        public IDialogCoordinator DialogCoordinator => throw new NotImplementedException();

        public string DialogTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsWide { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object OwnerContext => throw new NotImplementedException();

        public Task DisplayDialog(bool waitForClose = true)
        {
            throw new NotImplementedException();
        }

        public Task HideDialog()
        {
            throw new NotImplementedException();
        }

        public Task<IDialogResult> ShowDialogAsync(bool canCancel, bool waitForClose = true)
        {
            throw new NotImplementedException();
        }
    }
    public class TestDialogViewModel : IDialogViewModel
    {
        public IDialogContentView Dialog { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DialogTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IShellViewModel ShellViewModel => throw new NotImplementedException();

        public IDialogResult DialogResult { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CanAccept { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CanCancel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task DialogLoaded()
        {
            throw new NotImplementedException();
        }
    }
    public class TestFlyoutView : IFlyoutContentView { }
    public class TestFlyoutViewModel : IFlyoutViewModel { }
    public class TestAppSettings : IAppSettings
    {
        public string ConnectionString { get; set; } = "DataSource=:memory:";
    }
    public class TestApplicationConfiguration : IApplicationConfiguration
    {
        public IAppSettings AppSettings { get; set; } = new TestAppSettings();
    }
    public class TestRepository : IRepository
    {
        public TestRepository(IConnectionFactory factory, IAppSettings settings, Serilog.ILogger logger) { }
        public DatabaseInitializationResult InitializeDatabase()
        {
            throw new NotImplementedException();
        }

        public ILogger Logger { get; }
    }
}