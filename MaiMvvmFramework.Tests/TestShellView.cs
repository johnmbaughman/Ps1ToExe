using MahApps.Metro.Controls.Dialogs;
using MaiMvvmFramework.ViewModels;
using MaiMvvmFramework.Views;

namespace MaiMvvmFramework.Tests;

public class TestShellView : IShellView, IShellContentView
{
    public IShellContentView MainContentView { get; set; }
    public IDialogCoordinator DialogCoordinator { get; set; }
    public ShellViewModel ViewModel { get; }
    public IShellContentViewModel ShellContentViewModel { get; }
}