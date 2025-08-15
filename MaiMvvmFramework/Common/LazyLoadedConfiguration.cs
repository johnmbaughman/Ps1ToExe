namespace MaiMvvmFramework.Common;

/// <summary>
/// Represents an abstract base class for a configuration that is loaded lazily.
/// </summary>
public abstract class LazyLoadedConfiguration : ILazyLoadedConfiguration
{
    /// <summary>
    /// Gets or sets the local configuration.
    /// </summary>
    public ILocalConfiguration LocalConfiguration { get; set; } = null!;
}