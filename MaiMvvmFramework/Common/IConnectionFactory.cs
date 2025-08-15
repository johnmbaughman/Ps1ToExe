using System.Data;

namespace MaiMvvmFramework.Common;

/// <summary>
/// Defines a contract for creating and managing database connections.
/// Implementations provide connection string, provider information, and methods to open connections.
/// </summary>
public interface IConnectionFactory
{
    /// <summary>
    /// Gets the connection string.
    /// </summary>
    /// <value>The connection string.</value>
    string ConnectionString { get; }

    /// <summary>Gets the name of the provider.</summary>
    /// <value>The name of the provider.</value>
    string ProviderName { get; }

    /// <summary>
    /// Gets or sets the server name for the database connection.
    /// </summary>
    string ServerName { get; set; }

    /// <summary>
    /// Gets or sets the database name for the connection.
    /// </summary>
    string Database { get; set; }

    /// <summary>
    /// Asynchronously opens and returns a new database connection.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with a result of an open <see cref="IDbConnection"/>.</returns>
    Task<IDbConnection> OpenConnectionAsync();

    /// <summary>
    /// Opens and returns a new database connection.
    /// </summary>
    /// <returns>An open <see cref="IDbConnection"/>.</returns>
    IDbConnection OpenConnection();
}