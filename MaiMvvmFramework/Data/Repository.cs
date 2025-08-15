using MaiMvvmFramework.Common;
using Serilog;

namespace MaiMvvmFramework.Data;

/// <summary>
/// Abstract base class for repositories. 
/// Provides basic production data access for all Shop Floor Applications.
/// </summary>
/// <typeparam name="TRepo">
/// The repository type. Must implement <see cref="IRepository"/>.
/// </typeparam>
public abstract class Repository<TRepo> : IRepository where TRepo : class, IRepository
{
    /// <summary>
    /// Gets the connection factory used to create database connections.
    /// </summary>
    public IConnectionFactory ConnectionFactory { get; }

    /// <summary>
    /// Gets the logger instance for this repository.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TRepo}"/> class.
    /// </summary>
    /// <param name="connectionFactory">
    /// The connection factory used to create database connections.
    /// </param>
    /// <param name="logger">
    /// The logger used for logging repository operations.
    /// </param>
    protected Repository(IConnectionFactory connectionFactory, ILogger logger)
    {
        ConnectionFactory = connectionFactory;
        Logger = logger.ForContext(GetType());
    }

    /// <summary>
    /// Initializes the database for the repository.
    /// </summary>
    /// <returns>
    /// A <see cref="DatabaseInitializationResult"/> indicating the outcome of the initialization.
    /// </returns>
    public DatabaseInitializationResult InitializeDatabase()
    {
        return DatabaseInitializer.DeployDatabase<TRepo>(ConnectionFactory.ConnectionString);
    }
}