using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using MaiMvvmFramework.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MaiMvvmFramework.ViewModels;

/// <summary>
/// Represents the view model for the application shell.
/// Provides access to the main content view and its associated view model.
/// </summary>
public sealed partial class ShellViewModel : ViewModel, IShellViewModel
{
    private readonly IDialogCoordinator _dialogCoordinator;

    /// <summary>
    /// Gets or sets the main content view model.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IShellContentViewModel"/> representing the main content view model.
    /// </value>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDirty))]
    private IShellContentViewModel _mainContentViewModel;

    /// <summary>
    /// Gets or sets the title of the shell window.
    /// This property is typically displayed in the window's title bar and can be updated to reflect the current context or content.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsDirty))]
    private string _title;

    /// <summary>
    /// Gets the main content view displayed within the shell.
    /// </summary>
    /// <value>
    /// An instance of <see cref="IShellContentView"/> representing the main content view.
    /// </value>
    public IShellContentView MainContentView { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShellViewModel" /> class.
    /// </summary>
    /// <param name="mainContentView">The main content view to be displayed in the shell.</param>
    /// <param name="dialogCoordinator">The dialog coordinator used for managing dialogs.</param>
    /// <param name="applicationName">
    /// The name of the application, injected from keyed services.
    /// Used for context-specific configuration or display purposes.
    /// </param>
    public ShellViewModel(IShellContentView mainContentView, IDialogCoordinator dialogCoordinator, [FromKeyedServices("ApplicationName")] string applicationName)
    {
        Title = applicationName;

        _dialogCoordinator = dialogCoordinator;

        MainContentView = mainContentView;

        MainContentViewModel = MainContentView.ShellContentViewModel;

    }

    /// <summary>
    /// Handles logic when the view associated with this ShellViewModel is loaded.
    /// Assigns the shell view model to the main content view model and registers messenger receivers.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    public override Task ViewLoaded()
    {
        MainContentViewModel.AssignShellViewModel(this);
        MainContentViewModel.RegisterMessengerReceivers();

        return base.ViewLoaded();
    }

    /// <summary>
    /// Registers messenger receivers for inter-ViewModel communication.
    /// <para>
    /// Called by <c>ShellViewModel</c> during construction.
    /// Handlers should process minimal data; major data processing should be completed elsewhere.
    /// </para>
    /// </summary>
    public override void RegisterMessengerReceivers()
    {

    }
}