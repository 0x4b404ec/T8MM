
using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
using FluentAvalonia.UI.Windowing;
using T8MM.Pages.Splash;

namespace T8MM;

public partial class T8MMView : AppWindow
{
    public T8MMView()
    {
        InitializeComponent();

        SplashScreen = new ComplexSplashScreen();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        Show(); 
    }
}


internal class ComplexSplashScreen : IApplicationSplashScreen
{
    public ComplexSplashScreen()
    {
        SplashScreenContent = new SplashScreen();
    }

    public async Task RunTasks(CancellationToken cancellationToken)
    {
        await ((SplashScreen)SplashScreenContent).InitApp();
    }

    public string AppName { get; }
    public IImage AppIcon { get; }
    public object SplashScreenContent { get; }
    public int MinimumShowTime { get; }
}