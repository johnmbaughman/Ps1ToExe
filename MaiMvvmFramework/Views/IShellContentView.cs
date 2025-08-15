using MaiMvvmFramework.ViewModels;

namespace MaiMvvmFramework.Views;

/// <summary>
/// Defines the contract for a shell content view in the MaiMvvmFramework.
/// Provides access to the associated shell content view model.
/// </summary>
public interface IShellContentView
{
    /// <summary>
    /// Gets the view model associated with this shell content view.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IShellContentViewModel"/> representing the shell content view model.
    /// </value>
    IShellContentViewModel ShellContentViewModel { get; }
}