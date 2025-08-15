using System.Reflection;

namespace MaiMvvmFramework;

/// <summary>
/// Provides cached access to application metadata such as product name, version, company, and copyright.
/// </summary>
public static class ApplicationInfo
{
    private static bool _productNameCached;
    private static string _productName = null!;
    private static bool _versionCached;
    private static string _version = null!;
    private static bool _companyCached;
    private static string _company = null!;
    private static bool _copyrightCached;
    private static string _copyright = null!;

    /// <summary>
    /// Gets or sets the product name of the application.
    /// The value is retrieved from the <see cref="AssemblyProductAttribute"/> of the entry assembly and cached for future access.
    /// </summary>
    public static string ProductName
    {
        get
        {
            if (_productNameCached) return _productName;

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = (AssemblyProductAttribute)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyProductAttribute))!;
                _productName = attribute.Product;
            }
            else
                _productName = "";
            _productNameCached = true;
            return _productName;
        }

        set
        {
            _productName = value;
            _productNameCached = true;
        }
    }

    /// <summary>
    /// Gets the version number of the application.
    /// The value is retrieved from the entry assembly's <see cref="AssemblyName.Version"/> and cached for future access.
    /// </summary>
    public static string Version
    {
        get
        {
            if (_versionCached) return _version;

            var entryAssembly = Assembly.GetEntryAssembly();
            _version = entryAssembly != null ? entryAssembly.GetName().Version!.ToString() : "";

            _versionCached = true;
            return _version;
        }
    }

    /// <summary>
    /// Gets the company name of the application.
    /// The value is retrieved from the <see cref="AssemblyCompanyAttribute"/> of the entry assembly and cached for future access.
    /// </summary>
    public static string Company
    {
        get
        {
            if (_companyCached) return _company;

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyCompanyAttribute))!;
                _company = attribute.Company;
            }
            else
                _company = "";
            _companyCached = true;
            return _company;
        }
    }

    /// <summary>
    /// Gets the copyright information of the application.
    /// The value is retrieved from the <see cref="AssemblyCopyrightAttribute"/> of the entry assembly and cached for future access.
    /// </summary>
    public static string Copyright
    {
        get
        {
            if (_copyrightCached) return _copyright;

            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var attribute = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(entryAssembly, typeof(AssemblyCopyrightAttribute))!;
                _copyright = attribute.Copyright;
            }
            else
                _copyright = "";
            _copyrightCached = true;
            return _copyright;
        }
    }
}

