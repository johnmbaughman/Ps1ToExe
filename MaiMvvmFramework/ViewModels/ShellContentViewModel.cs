using MaiMvvmFramework.Views;

namespace MaiMvvmFramework.ViewModels;

/// <summary>
/// Abstract base class for shell content view models in the application.
/// Provides access to the associated view, the shell view model, and dialog context.
/// </summary>
public abstract class ShellContentViewModel : ViewModel, IShellContentViewModel
{
    private IShellViewModel _shellViewModel = null!;

    /// <summary>
    /// Gets or sets the owner context for dialogs.
    /// Used to associate dialogs with a specific context or parent.
    /// </summary>
    /// <value>
    /// The owner context object, or <c>null</c> if not set.
    /// </value>
    public object? DialogOwnerContext { get; set; }

    /// <summary>
    /// Gets or sets the shell view model that owns this content.
    /// </summary>
    /// <value>
    /// The <see cref="IShellViewModel"/> instance representing the shell.
    /// </value>
    public IShellViewModel ShellViewModel
    {
        get => _shellViewModel;
        set => SetProperty(ref _shellViewModel, value);
    }

    /// <summary>
    /// Gets or sets the view associated with this shell content view model.
    /// </summary>
    /// <value>
    /// The <see cref="IShellContentView"/> instance representing the view.
    /// </value>
    public IShellContentView View { get; set; } = null!;

    /// <summary>
    /// Assigns the shell view model to this content view model.
    /// Typically called during initialization to establish ownership.
    /// </summary>
    /// <param name="shellViewModel">
    /// The <see cref="IShellViewModel"/> instance to assign.
    /// </param>
    public virtual void AssignShellViewModel(IShellViewModel shellViewModel)
    {
        ShellViewModel = shellViewModel;
    }
}