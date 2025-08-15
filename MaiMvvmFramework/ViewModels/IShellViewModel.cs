using MaiMvvmFramework.Views;

namespace MaiMvvmFramework.ViewModels;

/// <summary>
/// Represents the view model for the application shell.
/// Provides access to the main content view and its associated view model.
/// </summary>
public interface IShellViewModel
{
    /// <summary>
    /// Gets the main content view displayed within the shell.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IShellContentView"/> representing the main content view.
    /// </value>
    IShellContentView MainContentView { get; }

    /// <summary>
    /// Gets the view model associated with the main content view.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IShellContentViewModel"/> representing the main content view model.
    /// </value>
    IShellContentViewModel MainContentViewModel { get; }
}