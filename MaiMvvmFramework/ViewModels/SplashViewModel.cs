namespace MaiMvvmFramework.ViewModels;

/// <summary>
/// Represents the view model for the splash screen in a WPF application.
/// Provides application information and startup messaging for the splash view.
/// </summary>
internal sealed class SplashViewModel : ViewModel, ISplashViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SplashViewModel"/> class.
    /// Sets up the splash screen with product name, version, and a startup message.
    /// </summary>
    /// <param name="message">Optional message to display on the splash screen. If null, a default loading message is used.</param>
    public SplashViewModel(string? message)
    {
        Logger?.Debug("Starting splash screen");
        if (!InDesignMode)
            return;

        Title = ApplicationInfo.ProductName;
        Message = message ?? "Loading initial data...";
        Version = ApplicationInfo.Version;

        Logger?.Information("Starting {ProductName}, version {Version}", ApplicationInfo.ProductName, ApplicationInfo.Version);
    }

    /// <summary>
    /// Registers messenger receivers for inter-view model communication.
    /// No receivers are registered for the splash screen.
    /// </summary>
    public override void RegisterMessengerReceivers() { }

    /// <summary>
    /// Gets the copyright string for the application.
    /// Format: "© {CurrentYear} JBS SA"
    /// </summary>
    public string Copyright => $"© {DateTime.Today.Year} JBS SA";

    /// <summary>
    /// Gets the message displayed on the splash screen.
    /// </summary>
    /// <value>
    /// The message shown to the user, such as loading status or tips.
    /// </value>
    public string? Message { get; }

    /// <summary>
    /// Gets the application title displayed on the splash screen.
    /// </summary>
    /// <value>
    /// The product name of the application.
    /// </value>
    public string? Title { get; }

    /// <summary>
    /// Gets the version string displayed on the splash screen.
    /// </summary>
    /// <value>
    /// The preformatted version number for display.
    /// </value>
    public string? Version { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to handle print server startup.
    /// </summary>
    /// <value>
    /// <c>true</c> if the print server should be loaded; otherwise, <c>false</c>.
    /// </value>
    public bool HandlePrinterServerStartUp { get; set; }
}

