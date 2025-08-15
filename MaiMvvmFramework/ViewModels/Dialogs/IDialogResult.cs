namespace MaiMvvmFramework.ViewModels.Dialogs;

/// <summary>
/// Represents the result of a dialog interaction.
/// </summary>
public interface IDialogResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the dialog was cancelled by the user.
    /// Returns <c>true</c> if the dialog was cancelled; otherwise, <c>false</c>.
    /// </summary>
    bool IsCancelled { get; set; }
}