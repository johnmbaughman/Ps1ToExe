using MaiMvvmFramework.ViewModels;

namespace MaiMvvmFramework.Views;

/// <summary>
/// Represents the contract for the splash window view in the application startup sequence.
/// </summary>
/// <remarks>
/// This interface is implemented internally by the framework. Consumers should not implement this interface directly.
/// </remarks>
internal interface ISplashView
{
    /// <summary>
    /// Gets the view model associated with the splash window.
    /// </summary>
    /// <value>
    /// An instance of <see cref="ISplashViewModel"/> representing the splash window's view model.
    /// </value>
    ISplashViewModel ViewModel { get; }
}