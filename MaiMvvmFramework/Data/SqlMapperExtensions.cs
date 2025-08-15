using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using Dapper;

namespace MaiMvvmFramework.Data;

/// <summary>
/// Class SqlMapperExtensions.
/// </summary>
public static class SqlMapperExtensions
{
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new();
    private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new();

    /// <summary>
    /// Merges a record set and a table to identify differences and then inserts, updates or deletes records.
    /// </summary>
    /// <typeparam name="T">The entity type being merged into the table.</typeparam>
    /// <param name="connection">The connection.</param>
    /// <param name="collectionToMerge">The collection to merge.</param>
    /// <param name="transaction">The transaction.</param>
    /// <param name="commandTimeout">The command timeout.</param>
    /// <param name="deleteTargetOnlyRecords">Whether target records that doesn't exist on source will be deleted.</param>
    /// <returns>The number of rows affected.</returns>
    /// <exception cref="System.ArgumentException">Entity must have at least one [Key] property</exception>
    public static int FullMerge<T>(this IDbConnection connection, IEnumerable<T> collectionToMerge,
        IDbTransaction? transaction = null, int? commandTimeout = null, bool deleteTargetOnlyRecords = true) where T : class
    {
        return FullMerge(connection, collectionToMerge, null, null, transaction, commandTimeout, deleteTargetOnlyRecords);
    }

    /// <summary>
    /// Merges a record set and a table to identify differences and then inserts, updates or deletes records.
    /// </summary>
    /// <typeparam name="T">The entity type being merged into the table.</typeparam>
    /// <param name="connection">The connection.</param>
    /// <param name="collectionToMerge">The collection to merge.</param>
    /// <param name="targetTableName">Name of the target table.</param>
    /// <param name="keys">entity keys to use on merge</param>
    /// <param name="transaction">The transaction.</param>
    /// <param name="commandTimeout">The command timeout.</param>
    /// <param name="deleteTargetOnlyRecords">Whether target records that doesn't exist on source will be deleted.</param>
    /// <returns>The number of rows affected.</returns>
    /// <exception cref="System.ArgumentNullException">The parameter is null and execution cannot continue</exception>
    /// <exception cref="System.ArgumentException">Entity must have at least one [Key] property</exception>
    public static int FullMerge<T>(this IDbConnection connection, IEnumerable<T> collectionToMerge, string? targetTableName, IEnumerable<PropertyInfo>? keys,
        IDbTransaction? transaction = null, int? commandTimeout = null, bool deleteTargetOnlyRecords = true) where T : class
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(collectionToMerge);
        ArgumentNullException.ThrowIfNull(targetTableName);
        ArgumentNullException.ThrowIfNull(keys);

        var type = typeof(T);
        keys = keys.ToList();
        var keyProperties = (keys.Any() ? keys : KeyPropertiesCache(type).ToList()).ToList();
        if (keyProperties.Count == 0)
            throw new ArgumentException("Entity must have at least one [Key] property");

        var allProperties = TypePropertiesCache(type).ToList();
        var allFieldsWithComma = string.Join(", ", allProperties.Select(item => $"[{item.Name}]"));
        var allFieldsWithTypeAndComma = string.Join(", ", allProperties.Select(item => $"[{item.Name}] {GetSqlType(item.PropertyType)}"));
        var nonIdProps = allProperties.Where(a => !keyProperties.Contains(a)).ToList();

        var sb = new StringBuilder();

        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "DECLARE @Source TABLE ({0})", allFieldsWithTypeAndComma));

        foreach (var record in collectionToMerge)
            sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "INSERT INTO @Source ({0}) VALUES ({1})", allFieldsWithComma, AllValues(record)));

        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "MERGE {0} AS Target", targetTableName));
        sb.AppendLine("USING @Source AS Source");
        sb.Append("ON (");

        for (var i = 0; i < keyProperties.Count; i++)
        {
            var property = keyProperties.ElementAt(i);
            sb.AppendFormat(CultureInfo.CurrentCulture, "Target.{0} = Source.{0}", property.Name);
            if (i < keyProperties.Count - 1)
                sb.AppendFormat(CultureInfo.CurrentCulture, " and ");
        }
        sb.AppendLine(")");

        var allSourceFieldsWithComma = string.Join(", ", allProperties.Select(item => $"Source.[{item.Name}]"));

        sb.AppendLine("WHEN NOT MATCHED BY TARGET");
        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "THEN INSERT({0})", allFieldsWithComma));
        sb.AppendLine(string.Format(CultureInfo.CurrentCulture, "VALUES({0})", allSourceFieldsWithComma));
        if (nonIdProps.Count != 0)
        {
            sb.AppendLine("WHEN MATCHED");
            sb.Append("THEN UPDATE SET ");

            for (var i = 0; i < nonIdProps.Count; i++)
            {
                var property = nonIdProps.ElementAt(i);
                sb.AppendFormat(CultureInfo.CurrentCulture, "Target.[{0}] = Source.[{0}]", property.Name);
                if (i < nonIdProps.Count - 1)
                    sb.AppendLine(", ");
            }
        }
        sb.AppendLine("");

        if (deleteTargetOnlyRecords)
        {
            sb.AppendLine("WHEN NOT MATCHED BY SOURCE");
            sb.AppendLine("THEN DELETE;");
        }
        else
        {
            sb.Append(';');
        }

        return connection.Execute(sb.ToString(), commandTimeout: commandTimeout, transaction: transaction);
        string AllValues(T record) => string.Join(", ", allProperties.Select(item => $"{GetSqlValueAsString(item.GetValue(record))}"));
    }

    private static string GetSqlType(Type type)
    {
        if (type == typeof(int)) return "int";
        if (type == typeof(string)) return "varchar(max)";
        if (type == typeof(Guid)) return "uniqueidentifier";
        if (type == typeof(bool)) return "bit";
        if (type == typeof(decimal)) return "decimal(10,4)";
        if (type == typeof(DateTime)) return "datetime";

        return "varchar(max)";
    }

    private static string GetSqlValueAsString(object? value)
    {
        if (value == null) return "NULL";

        var stringValue = value.ToString()?.Replace("'", "''");

        return value switch
        {
            string or Guid => string.Format(CultureInfo.CurrentCulture, "'{0}'", stringValue),
            bool bl => bl ? "1" : "0",
            DateTime dt => string.Format(CultureInfo.CurrentCulture, "'{0}'",
                dt.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.CurrentCulture)),
            _ => value.ToString() ?? string.Empty
        };
    }

    private static IEnumerable<PropertyInfo> KeyPropertiesCache(Type type)
    {
        if (KeyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo>? cache))
        {
            return cache;
        }

        var allProperties = TypePropertiesCache(type).ToList();
        var keyProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

        if (keyProperties.Count == 0)
        {
            var idProp = allProperties.FirstOrDefault(p => p.Name.ToLower(CultureInfo.CurrentCulture) == "id");
            if (idProp != null)
            {
                keyProperties.Add(idProp);
            }
        }

        KeyProperties[type.TypeHandle] = keyProperties;
        return keyProperties;
    }

    private static IEnumerable<PropertyInfo> TypePropertiesCache(Type type)
    {
        if (TypeProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo>? cache))
        {
            return cache;
        }

        var properties = type.GetProperties();
        TypeProperties[type.TypeHandle] = properties;
        return properties;
    }
}