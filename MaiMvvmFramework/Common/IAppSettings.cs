namespace MaiMvvmFramework.Common;

/// <summary>
/// Provides application configuration settings.
/// </summary>
public interface IAppSettings
{
    /// <summary>
    /// Gets or sets the database connection string.
    /// </summary>
    /// <value>
    /// The connection string used to access the database.
    /// </value>
    string ConnectionString { get; set; }
}