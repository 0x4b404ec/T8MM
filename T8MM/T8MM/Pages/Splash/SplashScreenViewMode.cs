    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using T8MM.ViewModels;

namespace T8MM.Pages.Splash;

public partial class SplashScreenViewMode : ViewModelBase
{
    [ObservableProperty]
    private int m_progressValue;
    
    [ObservableProperty]
    private string m_startupMessage = "Starting application...";

    private readonly CancellationTokenSource m_cts = new();

    public CancellationToken CancellationToken => m_cts.Token;
    
    public void Cancel()
    {
        StartupMessage = "Cancelling...";
        m_cts.Cancel();
    }
}