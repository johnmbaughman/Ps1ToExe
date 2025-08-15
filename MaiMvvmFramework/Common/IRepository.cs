using MaiMvvmFramework.Data;
using Serilog;

namespace MaiMvvmFramework.Common;

/// <summary>
/// Defines the contract for a repository that manages database initialization and logging.
/// </summary>
public interface IRepository
{
    /// <summary>
    /// Initializes the database and returns the result of the operation.
    /// </summary>
    /// <returns>
    /// A <see cref="DatabaseInitializationResult"/> containing information about the success or failure of the initialization,
    /// any exception encountered, and the script that caused the error if applicable.
    /// </returns>
    DatabaseInitializationResult InitializeDatabase();

    /// <summary>
    /// Gets the logger used by the repository for diagnostic and operational logging.
    /// </summary>
    /// <value>
    /// An instance of <see cref="ILogger"/> for logging repository events.
    /// </value>
    ILogger Logger { get; }
}