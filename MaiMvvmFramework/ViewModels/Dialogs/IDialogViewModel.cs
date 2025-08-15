using MaiMvvmFramework.Views.Dialogs;

namespace MaiMvvmFramework.ViewModels.Dialogs;

/// <summary>
/// Represents the contract for a dialog view model in the ShopFloor WPF application.
/// Provides properties and methods for dialog content, title, result, and interaction state.
/// </summary>
public interface IDialogViewModel
{
    /// <summary>
    /// Gets or sets the dialog content view associated with this view model.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IDialogContentView"/> representing the dialog content.
    /// </value>
    IDialogContentView Dialog { get; set; }

    /// <summary>
    /// Called when the dialog is loaded. Used for initialization logic.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task DialogLoaded();

    /// <summary>
    /// Gets or sets the title of the dialog.
    /// </summary>
    /// <value>
    /// The dialog title as a <see cref="string"/>.
    /// </value>
    string DialogTitle { get; set; }

    /// <summary>
    /// Gets the shell view model associated with this dialog.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IShellViewModel"/> representing the shell view model.
    /// </value>
    IShellViewModel ShellViewModel { get; }

    /// <summary>
    /// Gets or sets the result of the dialog interaction.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IDialogResult"/> representing the dialog result.
    /// </value>
    IDialogResult DialogResult { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog can be accepted.
    /// </summary>
    /// <value>
    /// <c>true</c> if the dialog can be accepted; otherwise, <c>false</c>.
    /// </value>
    bool CanAccept { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog can be cancelled.
    /// </summary>
    /// <value>
    /// <c>true</c> if the dialog can be cancelled; otherwise, <c>false</c>.
    /// </value>
    bool CanCancel { get; set; }
}