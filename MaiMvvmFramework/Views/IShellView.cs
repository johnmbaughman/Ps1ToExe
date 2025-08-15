using MaiMvvmFramework.ViewModels;

namespace MaiMvvmFramework.Views;

/// <summary>
/// Defines the contract for the main shell view in the MaiMvvmFramework WPF application.
/// Provides access to the associated <see cref="ShellViewModel"/> instance, which manages
/// the main content view and its view model.
/// </summary>
public interface IShellView
{
    /// <summary>
    /// Gets the <see cref="ShellViewModel"/> associated with this shell view.
    /// The shell view model provides access to the main content view and its view model,
    /// and coordinates dialog management and inter-ViewModel communication.
    /// </summary>
    /// <value>
    /// An instance of <see cref="ShellViewModel"/> representing the shell's view model.
    /// </value>
    ShellViewModel ViewModel { get; }
}