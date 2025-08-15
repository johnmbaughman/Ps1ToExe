namespace MaiMvvmFramework.Common;

/// <summary>
/// Represents the application configuration, providing access to application settings.
/// </summary>
public interface IApplicationConfiguration
{
    /// <summary>
    /// Gets or sets the application settings.
    /// </summary>
    IAppSettings AppSettings { get; set; }
}