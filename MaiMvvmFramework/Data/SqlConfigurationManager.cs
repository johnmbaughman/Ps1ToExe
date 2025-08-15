using Dapper;
using System.Globalization;
using MaiMvvmFramework.Hosting;
using MaiMvvmFramework.Common;

namespace MaiMvvmFramework.Data;

/// <summary>
/// Configuration manager contract
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IConfigurationManager<out T> where T : IApplicationConfiguration
{
    /// <summary>
    /// Gets a configuration object.
    /// </summary>
    /// <returns></returns>
    T Get();
}

/// <summary>
/// Provides Configuration fetching from database
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class SqlConfigurationManager<T> : IConfigurationManager<T> where T : class, IApplicationConfiguration, new()
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly string _configurationTableName;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlConfigurationManager{T}" /> class.
    /// </summary>
    /// <param name="connectionFactory">The connection factory.</param>
    /// <param name="configurationTableName">Name of the configuration table.</param>
    public SqlConfigurationManager(IConnectionFactory connectionFactory, string configurationTableName = "ScaleConfiguration")
    {
        _connectionFactory = connectionFactory;
        _configurationTableName = configurationTableName;
    }

    /// <summary>
    /// Gets this instance.
    /// </summary>
    /// <returns></returns>
    public T Get()
    {
        using var db = _connectionFactory.OpenConnection();
        const string sql = """
                           SELECT [Code]
                           ,[Value]
                             FROM [dbo].[{0}]
                           """;

        var dictionaryResult = db.Query<ConfigurationModel>(string.Format(CultureInfo.CurrentCulture, sql, _configurationTableName))
            .Where(item => !string.IsNullOrEmpty(item.Code))
            .ToDictionary(item => item.Code, item => item.Value.ToString());

        var results = dictionaryResult.CreateGenericInstanceFromDictionary<T>();
        return results;
    }
}

public class ConfigurationModel
{
    public string Code { get; set; } = null!;
    public string Value { get; set; } = null!;
}