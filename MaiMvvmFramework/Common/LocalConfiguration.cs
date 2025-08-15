namespace MaiMvvmFramework.Common;

/// <summary>
/// Provides local configuration settings for the application, including database connection string,
/// application name, and the current workstation DNS name.
/// </summary>
public class LocalConfiguration : ILocalConfiguration
{
    /// <summary>
    /// Gets or sets the local database connection string.
    /// </summary>
    public string LocalDbConnectionString { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the DNS name of the current workstation.
    /// Defaults to the machine name.
    /// </summary>
    public string CurrentDnsName { get; set; } = Environment.MachineName;
}