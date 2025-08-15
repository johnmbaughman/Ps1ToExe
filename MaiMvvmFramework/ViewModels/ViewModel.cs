using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Serilog;

namespace MaiMvvmFramework.ViewModels;

public abstract partial class ViewModel : ObservableObject, IViewModel
{
    private bool _canAccept;
    private bool _canCancel;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModel" /> class.
    /// </summary>
    /// <exception cref="System.NullReferenceException">logger</exception>
    protected ViewModel()
    {
        Logger = MaiMvvmApplication.Logger?.ForContext(GetType()) ?? null;
    }

    /// <summary>
    /// The is busy
    /// </summary>
    [ObservableProperty]
    private bool _isBusy;

    /// <summary>
    /// The is dirty
    /// </summary>
    [ObservableProperty]
    private bool _isDirty;

    /// <summary>
    /// Gets or sets a value indicating whether this instance can accept.
    /// </summary>
    /// <value><c>true</c> if this instance can accept; otherwise, <c>false</c>.</value>
    public virtual bool CanAccept
    {
        get => _canAccept;
        set
        {
            SetProperty(ref _canAccept, value);
            IsDirty = true;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance can cancel.
    /// </summary>
    /// <value><c>true</c> if this instance can cancel; otherwise, <c>false</c>.</value>
    public virtual bool CanCancel
    {
        get => _canCancel;
        set
        {
            SetProperty(ref _canCancel, value);
            IsDirty = true;
        }
    }

    /// <summary>
    /// Gets a value indicating whether [in design mode].
    /// </summary>
    /// <value><c>true</c> if [in design mode]; otherwise, <c>false</c>.</value>
    public bool InDesignMode => Debugger.IsAttached;

    /// <summary>
    /// Gets the log factory.
    /// </summary>
    /// <value>The log factory.</value>
    public ILogger? Logger { get; }

    /// <summary>
    /// Registers the messenger receivers.
    /// </summary>
    /// <remarks>Called by ShellViewModel during constructor execution.
    /// <b>IMPORTANT:</b>Handlers should process minimal data. Major data processing should be completed elsewhere, not directly in this function.</remarks>
    public abstract void RegisterMessengerReceivers();

    /// <summary>
    /// Handles the <see cref="E:Loaded" /> event.
    /// </summary>
    /// <returns>Task.</returns>
    public virtual Task ViewLoaded()
    {
        return Task.CompletedTask;
    }
}