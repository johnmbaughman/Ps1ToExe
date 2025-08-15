using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MaiMvvmFramework.Hosting;

/// <summary>
/// Provides useful extensions
/// </summary>
public static class GeneralExtensions
{
    /// <summary>
    /// Attempts to retrieve a required service of type <typeparamref name="T"/> from the specified <see cref="IServiceProvider"/>.
    /// Returns <c>true</c> and sets <paramref name="service"/> if successful; otherwise returns <c>false</c> and sets <paramref name="service"/> to its default value.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve. Must be non-nullable.</typeparam>
    /// <param name="services">The <see cref="IServiceProvider"/> to retrieve the service from.</param>
    /// <param name="service">When this method returns, contains the service instance if found; otherwise, the default value for type <typeparamref name="T"/>.</param>
    /// <returns><c>true</c> if the service was successfully retrieved; otherwise, <c>false</c>.</returns>
    public static bool TryGetService<T>(this IServiceProvider services, out T service) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(services);
        service = default!;
        try
        {
            service = services.GetRequiredService<T>();
            return true;
        }
        catch (Exception)
        {
            // ignored
        }
        return false;
    }

    /// <summary>Creates the generic instance from dictionary.</summary>
    /// <typeparam name="T" ></typeparam>
    /// <param name="values" >The values.</param>
    /// <returns></returns>
    public static T CreateGenericInstanceFromDictionary<T>(this IEnumerable<KeyValuePair<string, string>> values) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(values);

        var result = new T();

        var properties = result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var entry in values)
        {
            var propertyToSet = properties.FirstOrDefault(item => item.Name == entry.Key && item.CanWrite);

            if (propertyToSet == null) continue;

            if (propertyToSet.PropertyType.IsValueType && !string.IsNullOrEmpty(entry.Value) ||
                propertyToSet.PropertyType == typeof(string))
            {
                var value = propertyToSet.PropertyType.IsEnum
                    ? Enum.Parse(propertyToSet.PropertyType, entry.Value)
                    : Convert.ChangeType(entry.Value, propertyToSet.PropertyType);
                propertyToSet.SetValue(result, value);
            }
            else if (entry.Value.IsJson())
            {
                var value = entry.Value.DeserializeJson(propertyToSet.PropertyType);

                propertyToSet.SetValue(result, value);
            }
        }

        return result;

    }

    /// <summary>Deserializes the json.</summary>
    /// <param name="input" >The input.</param>
    /// <param name="objectType" >Type of the object.</param>
    /// <returns></returns>
    public static object? DeserializeJson(this string input, Type objectType)
    {
        return JsonSerializer.Deserialize(input, objectType);
    }

    /// <summary>Gets the flag enum values.</summary>
    /// <typeparam name="TEnum" >The type of the enum.</typeparam>
    /// <param name="value" >The value.</param>
    /// <returns></returns>
    public static IEnumerable<TEnum> GetFlagEnumValues<TEnum>(this Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return Enum.GetValues(typeof(TEnum))
            .Cast<Enum>()
            .Where(value.HasFlag)
            .Cast<TEnum>();
    }

    /// <summary>Gets the reg ex match.</summary>
    /// <param name="input" >The input.</param>
    /// <param name="pattern" >The pattern.</param>
    /// <param name="groupName" >Name of the group.</param>
    /// <returns></returns>
    public static string GetRegExMatch(this string input, string pattern, string groupName)
    {
        var group = Regex.Match(input, pattern).Groups[groupName];
        return group.Value;
    }

    /// <summary>Determines whether the specified number is between.</summary>
    /// <param name="number" >The number.</param>
    /// <param name="firstNumber" >The first number.</param>
    /// <param name="lastNumber" >The last number.</param>
    /// <returns></returns>
    public static bool IsBetween(this int number, int firstNumber, int lastNumber)
    {
        return number >= firstNumber && number <= lastNumber;
    }

    /// <summary>Determines whether the specified input is json.</summary>
    /// <param name="input" >The input.</param>
    /// <returns></returns>
    public static bool IsJson(this string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var trimmedInput = input.Trim();

        return trimmedInput.StartsWith("{", StringComparison.OrdinalIgnoreCase) && trimmedInput.EndsWith("}", StringComparison.OrdinalIgnoreCase)
               || trimmedInput.StartsWith("[", StringComparison.OrdinalIgnoreCase) && trimmedInput.EndsWith("]", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>compares two strings and returns a boolean result</summary>
    /// <param name="first" >The first.</param>
    /// <param name="second" >The second.</param>
    /// <param name="comparison" >The comparison.</param>
    /// <returns></returns>
    public static bool Same(this string first, string second, StringComparison comparison = StringComparison.CurrentCulture)
    {
        return string.Compare(first, second, comparison) == 0;
    }

    /// <summary>Scans an object public properties and return them and their values into a dictionary</summary>
    /// <param name="value" >The value.</param>
    /// <returns></returns>
    public static IDictionary<string, object> ToPropertiesDictionary(this object? value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var propertiesQuery = from property in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            let browsableAttributes = property.GetCustomAttributes(typeof(BrowsableAttribute)).OfType<BrowsableAttribute>()
            where !browsableAttributes.Any()
                  || browsableAttributes.All(item => item.Browsable)
            select property;

        return propertiesQuery.ToDictionary(item => item.Name, item => item.GetValue(value) ?? throw new InvalidOperationException($"Property '{item.Name}' value is null."));
    }
}