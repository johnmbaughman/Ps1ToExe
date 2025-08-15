using MaiMvvmFramework.Views;

namespace MaiMvvmFramework.ViewModels;

/// <summary>
/// Represents a view model for shell content in the application.
/// Provides access to the associated view, shell view model, and dialog context.
/// </summary>
public interface IShellContentViewModel : IViewModel
{
    /// <summary>
    /// Gets or sets the owner context for dialogs.
    /// Used to associate dialogs with a specific context or parent.
    /// </summary>
    /// <value>
    /// The owner context object, or <c>null</c> if not set.
    /// </value>
    object? DialogOwnerContext { get; set; }

    /// <summary>
    /// Gets or sets the shell view model that owns this content.
    /// </summary>
    /// <value>
    /// The <see cref="IShellViewModel"/> instance representing the shell.
    /// </value>
    IShellViewModel ShellViewModel { get; set; }

    /// <summary>
    /// Gets or sets the view associated with this shell content view model.
    /// </summary>
    /// <value>
    /// The <see cref="IShellContentView"/> instance representing the view.
    /// </value>
    IShellContentView View { get; set; }

    /// <summary>
    /// Assigns the shell view model to this content view model.
    /// Typically called during initialization to establish ownership.
    /// </summary>
    /// <param name="shellViewModel">
    /// The <see cref="IShellViewModel"/> instance to assign.
    /// </param>
    void AssignShellViewModel(IShellViewModel shellViewModel);
}