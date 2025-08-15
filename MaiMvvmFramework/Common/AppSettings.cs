namespace MaiMvvmFramework.Common;

/// <summary>
/// Represents the base application settings for configuration management.
/// Implements <see cref="IAppSettings"/> to provide access to configuration values.
/// </summary>
public abstract class AppSettings : IAppSettings
{
    /// <summary>
    /// Gets or sets the database connection string.
    /// </summary>
    /// <value>
    /// The connection string used to access the database.
    /// </value>
    public string ConnectionString { get; set; } = null!;
}