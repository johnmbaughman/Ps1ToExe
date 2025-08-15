using DbUp;

namespace MaiMvvmFramework.Data;

/// <summary>
/// Provides methods to initialize and upgrade the application's database using embedded migration scripts.
/// </summary>
public class DatabaseInitializer
{
    /// <summary>
    /// Deploys the database by ensuring its existence and applying any required migrations.
    /// </summary>
    /// <typeparam name="T">
    /// The type whose assembly contains the embedded migration scripts.
    /// </typeparam>
    /// <param name="connectionString">
    /// The connection string to the target SQL database.
    /// </param>
    /// <returns>
    /// A <see cref="DatabaseInitializationResult"/> indicating the outcome of the deployment.
    /// </returns>
    public static DatabaseInitializationResult DeployDatabase<T>(string connectionString)
    {
        // Ensure the database exists.
        EnsureDatabase.For.SqlDatabase(connectionString);

        // Configure the upgrade engine to use embedded scripts from the specified assembly.
        var upgradeEngine =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(typeof(T).Assembly)
                .Build();

        // If no upgrade is required, return a successful result.
        if (!upgradeEngine.IsUpgradeRequired())
            return new DatabaseInitializationResult(true, null, string.Empty);

        // Perform the upgrade and return the result.
        var result = upgradeEngine.PerformUpgrade();
        return new DatabaseInitializationResult(result.Successful, result.Error, result.ErrorScript?.Contents);
    }
}