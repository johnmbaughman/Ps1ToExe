using ControlzEx.Standard;

namespace MaiMvvmFramework.Common;

public interface ILocalConfiguration
{
    /// <summary>
    /// Gets or sets the local database connection string.
    /// </summary>
    /// <value>The local database connection string.</value>
    string LocalDbConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the workstation application.
    /// </summary>
    /// <value>The workstation application.</value>
    string ApplicationName { get; set; }

    /// <summary>
    /// Gets or sets the name of the workstation DNS.
    /// </summary>
    /// <value>The name of the workstation DNS.</value>
    string CurrentDnsName { get; set; }
}