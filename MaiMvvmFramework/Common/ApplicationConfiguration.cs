namespace MaiMvvmFramework.Common;

/// <summary>
/// Represents the application configuration, providing access to application settings.
/// </summary>
/// <remarks>
/// This class is typically used to encapsulate configuration settings required by the application,
/// such as database connection strings and other environment-specific options.
/// </remarks>
public abstract class ApplicationConfiguration : IApplicationConfiguration
{
    /// <summary>
    /// Gets or sets the application settings.
    /// </summary>
    /// <value>
    /// An <see cref="IAppSettings"/> instance containing configuration values for the application.
    /// </value>
    public IAppSettings AppSettings { get; set; } = null!;
}