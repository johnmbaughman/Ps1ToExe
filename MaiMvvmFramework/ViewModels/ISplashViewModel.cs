namespace MaiMvvmFramework.ViewModels;

/// <summary>
/// Defines the contract for a splash screen view model in a WPF application.
/// </summary>
/// <remarks>
/// Implementations should provide properties for splash screen display and
/// register messenger receivers for inter-component communication using
/// <see href="https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/messenger">Community Toolkit Messenger</see>.
/// </remarks>
internal interface ISplashViewModel
{
    /// <summary>
    /// Registers messenger receivers to enable communication between components.
    /// </summary>
    void RegisterMessengerReceivers();

    /// <summary>
    /// Gets the copyright year displayed on the splash screen.
    /// </summary>
    /// <example>
    /// "© 2024"
    /// </example>
    string Copyright { get; }

    /// <summary>
    /// Gets the message displayed on the splash view, such as loading status or tips.
    /// </summary>
    /// <example>
    /// "Loading application modules..."
    /// </example>
    string? Message { get; }

    /// <summary>
    /// Gets the application title displayed on the splash screen.
    /// </summary>
    /// <example>
    /// "MaiMvvmFramework"
    /// </example>
    string? Title { get; }

    /// <summary>
    /// Gets the preformatted version number for display on the splash screen.
    /// </summary>
    /// <example>
    /// "v1.2.3"
    /// </example>
    string? Version { get; }
}