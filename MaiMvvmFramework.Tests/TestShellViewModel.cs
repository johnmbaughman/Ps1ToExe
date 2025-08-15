using MaiMvvmFramework.ViewModels;
using MaiMvvmFramework.Views;
using Serilog;

public class TestShellViewModel : IShellViewModel, IShellContentViewModel
{
    public IShellContentView MainContentView { get; }
    public IShellContentViewModel MainContentViewModel { get; }

    public TestShellViewModel(IShellContentView mainContentView, IShellContentViewModel mainContentViewModel)
    {
        MainContentView = mainContentView;
        MainContentViewModel = mainContentViewModel;
    }

    public bool IsBusy { get; set; }
    public bool IsDirty { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool InDesignMode { get; }
    public ILogger? Logger { get; }
    public void RegisterMessengerReceivers()
    {
        throw new NotImplementedException();
    }

    public Task ViewLoaded()
    {
        throw new NotImplementedException();
    }

    public object? DialogOwnerContext { get; set; }
    public IShellViewModel ShellViewModel { get; set; }
    public IShellContentView View { get; set; }
    public void AssignShellViewModel(IShellViewModel shellViewModel)
    {
        throw new NotImplementedException();
    }
}
