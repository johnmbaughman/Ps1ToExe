using Microsoft.Extensions.DependencyInjection;
using PowerShellToolsPro.Packager.ViewModels;

public partial class App : Application
{
    public static ServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        // Register ViewModels
        services.AddSingleton<WizardViewModel>();
        services.AddSingleton<WizardPage1ViewModel>();
        services.AddSingleton<WizardPage2ViewModel>();
        // ...add more pages as needed

        ServiceProvider = services.BuildServiceProvider();

        base.OnStartup(e);
    }
}