using MaiMvvmFramework;
using MaiMvvmFramework.ViewModels;
using MaiMvvmFramework.Views;

namespace Ps1ToExe.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IShellContentView
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = MaiMvvmApplication.ShellContentViewModel;
        Loaded += async (_, _) => await ShellContentViewModel.ViewLoaded();
    }

    public IShellContentViewModel ShellContentViewModel => (IShellContentViewModel)DataContext;
}