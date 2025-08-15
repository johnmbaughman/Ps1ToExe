namespace MaiMvvmFramework.Data;

/// <summary>
/// Represents the result of a database initialization or upgrade operation.
/// Contains information about success, any error encountered, and the script that caused the error.
/// </summary>
public sealed class DatabaseInitializationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseInitializationResult"/> class.
    /// </summary>
    /// <param name="successful">Indicates whether the database initialization or upgrade was successful.</param>
    /// <param name="error">The exception that occurred during initialization, or <c>null</c> if successful.</param>
    /// <param name="errorScript">The script that was executing when the error occurred, or <c>null</c> if successful.</param>
    public DatabaseInitializationResult(bool successful, Exception error, string errorScript)
    {
        Successful = successful;
        Error = error;
        ErrorScript = errorScript;
    }

    /// <summary>
    /// Gets a value indicating whether the database initialization or upgrade was successful.
    /// </summary>
    public bool Successful { get; }

    /// <summary>
    /// Gets the exception that occurred during initialization, or <c>null</c> if successful.
    /// </summary>
    public Exception Error { get; }

    /// <summary>
    /// Gets the script that was executing when an error occurred, or <c>null</c> if successful.
    /// </summary>
    public string ErrorScript { get; }
}