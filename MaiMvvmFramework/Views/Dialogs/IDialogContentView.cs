using MahApps.Metro.Controls.Dialogs;
using MaiMvvmFramework.ViewModels.Dialogs;

namespace MaiMvvmFramework.Views.Dialogs;

/// <summary>
/// Represents the contract for dialog content views used within the application.
/// Provides properties and methods for dialog coordination, display, and interaction.
/// </summary>
public interface IDialogContentView
{
    /// <summary>
    /// Gets the dialog coordinator responsible for managing dialog interactions.
    /// </summary>
    /// <value>
    /// The <see cref="IDialogCoordinator"/> instance used to coordinate dialogs.
    /// </value>
    IDialogCoordinator DialogCoordinator { get; }

    /// <summary>
    /// Gets or sets the title displayed on the dialog.
    /// </summary>
    /// <value>
    /// The dialog's title as a <see cref="string"/>.
    /// </value>
    string DialogTitle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog should be displayed in a wide format.
    /// </summary>
    /// <value>
    /// <c>true</c> if the dialog is wide; otherwise, <c>false</c>.
    /// </value>
    bool IsWide { get; set; }

    /// <summary>
    /// Gets the context object that owns or initiated the dialog.
    /// </summary>
    /// <value>
    /// The owner context as an <see cref="object"/>.
    /// </value>
    object OwnerContext { get; }

    /// <summary>
    /// Displays the dialog to the user.
    /// </summary>
    /// <param name="waitForClose">
    /// If <c>true</c>, the method will wait until the dialog is closed before returning.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task DisplayDialog(bool waitForClose = true);

    /// <summary>
    /// Hides the dialog from the user.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task HideDialog();

    /// <summary>
    /// Asynchronously shows the dialog, allowing for cancellation and optionally waiting for close.
    /// </summary>
    /// <param name="canCancel">
    /// If <c>true</c>, the dialog can be cancelled by the user.
    /// </param>
    /// <param name="waitForClose">
    /// If <c>true</c>, the method will wait until the dialog is closed before returning.
    /// </param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that returns an <see cref="IDialogResult"/> indicating the outcome of the dialog.
    /// </returns>
    Task<IDialogResult> ShowDialogAsync(bool canCancel, bool waitForClose = true);
}