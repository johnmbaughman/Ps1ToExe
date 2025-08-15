namespace MaiMvvmFramework.Common;

/// <summary>
/// Represents a configuration interface that supports lazy loading of local configuration settings.
/// </summary>
public interface ILazyLoadedConfiguration
{
    /// <summary>
    /// Gets or sets the local configuration instance.
    /// </summary>
    /// <value>
    /// An <see cref="ILocalConfiguration"/> object containing local configuration settings.
    /// </value>
    public ILocalConfiguration LocalConfiguration { get; set; }
}