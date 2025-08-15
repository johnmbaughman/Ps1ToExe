using Serilog;

namespace MaiMvvmFramework.ViewModels;

/// <summary>
/// Defines the contract for a ViewModel in the MaiMvvmFramework.
/// </summary>
public interface IViewModel
{
    /// <summary>
    /// Gets or sets a value indicating whether the ViewModel is performing a background operation.
    /// </summary>
    bool IsBusy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the ViewModel has unsaved changes.
    /// </summary>
    bool IsDirty { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the ViewModel can accept changes.
    /// </summary>
    /// <value><c>true</c> if this instance can accept; otherwise, <c>false</c>.</value>
    bool CanAccept { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the ViewModel can cancel changes.
    /// </summary>
    /// <value><c>true</c> if this instance can cancel; otherwise, <c>false</c>.</value>
    bool CanCancel { get; set; }

    /// <summary>
    /// Gets a value indicating whether the ViewModel is running in design mode.
    /// </summary>
    /// <value><c>true</c> if in design mode; otherwise, <c>false</c>.</value>
    bool InDesignMode { get; }

    /// <summary>
    /// Gets the logger instance for this ViewModel.
    /// </summary>
    /// <value>The logger.</value>
    ILogger? Logger { get; }

    /// <summary>
    /// Registers messenger receivers for inter-ViewModel communication.
    /// <para>
    /// Called by <c>ShellViewModel</c> during construction.
    /// Handlers should process minimal data; major data processing should be completed elsewhere.
    /// </para>
    /// </summary>
    void RegisterMessengerReceivers();

    /// <summary>
    /// Handles logic when the view associated with this ViewModel is loaded.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ViewLoaded();
}