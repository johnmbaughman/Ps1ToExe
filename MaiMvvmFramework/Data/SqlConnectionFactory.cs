using System.Data;
using System.Data.Common;
using MaiMvvmFramework.Common;
using Microsoft.Data.SqlClient;

namespace MaiMvvmFramework.Data;

/// <summary>
/// Provides a factory for creating and opening SQL Server database connections.
/// Implements <see cref="IConnectionFactory"/>.
/// </summary>
public class SqlConnectionFactory : IConnectionFactory
{
    /// <summary>
    /// The underlying database provider factory.
    /// </summary>
    private readonly DbProviderFactory _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlConnectionFactory"/> class.
    /// Parses the connection string to extract server and database names.
    /// </summary>
    /// <param name="connectionString">The connection string for the SQL Server database.</param>
    public SqlConnectionFactory(string connectionString)
    {
        ConnectionString = connectionString;
        ProviderName = "System.Data.SqlClient";
        _factory = SqlClientFactory.Instance;
        var connectionBuilder = new SqlConnectionStringBuilder(ConnectionString);
        ServerName = connectionBuilder.DataSource;
        Database = connectionBuilder.InitialCatalog;
    }

    /// <summary>
    /// Gets the connection string used to connect to the database.
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Gets the name of the database provider.
    /// </summary>
    public string ProviderName { get; }

    /// <summary>
    /// Gets or sets the SQL Server instance name.
    /// </summary>
    public string ServerName { get; set; }

    /// <summary>
    /// Gets or sets the database name.
    /// </summary>
    public string Database { get; set; }

    /// <summary>
    /// Asynchronously opens a new database connection.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{IDbConnection}"/> representing the asynchronous operation,
    /// with the opened <see cref="IDbConnection"/> as the result.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the connection could not be created.
    /// </exception>
    public async Task<IDbConnection> OpenConnectionAsync()
    {
        var connection = _factory.CreateConnection();

        if (connection == null) throw new InvalidOperationException();

        connection.ConnectionString = ConnectionString;

        await connection.OpenAsync();
        return connection;
    }

    /// <summary>
    /// Opens a new database connection synchronously.
    /// </summary>
    /// <returns>
    /// The opened <see cref="IDbConnection"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the connection could not be created.
    /// </exception>
    public IDbConnection OpenConnection()
    {
        var connection = _factory.CreateConnection();

        if (connection == null) throw new InvalidOperationException();

        connection.ConnectionString = ConnectionString;

        connection.Open();
        return connection;
    }
}