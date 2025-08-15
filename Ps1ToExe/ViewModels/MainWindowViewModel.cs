using MaiMvvmFramework.ViewModels;
using Serilog;

namespace Ps1ToExe.ViewModels;

public class MainWindowViewModel : ShellContentViewModel
{
    public MainWindowViewModel()
    {
        Logger.Debug("In constructor");
        Logger.Debug("Leaving constructor");
    }

    public override void RegisterMessengerReceivers()
    {

    }
}