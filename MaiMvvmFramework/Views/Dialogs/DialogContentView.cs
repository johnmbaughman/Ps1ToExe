using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using MaiMvvmFramework.ViewModels.Dialogs;

namespace MaiMvvmFramework.Views.Dialogs;

/// <summary>
/// Provides a base implementation for dialog content views in a WPF application using MahApps.Metro.
/// Inherits from <see cref="BaseMetroDialog"/> and implements <see cref="IDialogContentView"/>.
/// Supports dialog coordination, display, and interaction.
/// </summary>
/// <remarks>
/// This class is intended to be used as a base for custom dialog content views.
/// It provides properties and methods for dialog display, hiding, and asynchronous interaction.
/// </remarks>
/// <seealso cref="BaseMetroDialog"/>
/// <seealso cref="IDialogContentView"/>
public abstract class DialogContentView : BaseMetroDialog, IDialogContentView
{
    /// <summary>
    /// The is wide property
    /// </summary>
    public static readonly DependencyProperty IsWideProperty = DependencyProperty.RegisterAttached("IsWide", typeof(bool), typeof(DialogContentView), new FrameworkPropertyMetadata(false));

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogContentView" /> class.
    /// </summary>
    /// <param name="dialogCoordinator">The dialog coordinator.</param>
    /// <param name="ownerContext">The owner context.</param>
    /// <param name="isWide">if set to <c>true</c> [is wide].</param>
    protected DialogContentView(IDialogCoordinator dialogCoordinator, object ownerContext, bool isWide = true)
    {
        DialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
        OwnerContext = ownerContext ?? throw new ArgumentNullException(nameof(ownerContext));
        IsWide = isWide;
    }

    /// <inheritdoc/>
    public IDialogCoordinator DialogCoordinator { get; }

    /// <inheritdoc/>
    public virtual string DialogTitle { get; set; } = null!;

    /// <inheritdoc/>
    public bool IsWide
    {
        get => (bool)GetValue(IsWideProperty);
        set => SetValue(IsWideProperty, value);
    }

    /// <inheritdoc/>
    public object OwnerContext { get; }

    /// <inheritdoc/>
    public async Task DisplayDialog(bool waitForClose = true)
    {
        var dialogSettings = new MetroDialogSettings { OwnerCanCloseWithDialog = true };
        await DialogCoordinator.ShowMetroDialogAsync(OwnerContext, this, dialogSettings);
        if (waitForClose)
        {
            await WaitUntilUnloadedAsync().ConfigureAwait(true);
        }
    }

    /// <inheritdoc/>
    public async Task HideDialog()
    {
        await DialogCoordinator.HideMetroDialogAsync(OwnerContext, this);
    }

    /// <inheritdoc/>
    public abstract Task<IDialogResult> ShowDialogAsync(bool canCancel, bool waitForClose = true);
}