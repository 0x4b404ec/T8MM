using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using T8MM.Pages;
using T8MM.Pages.Splash;
using T8MM.Services;
using T8MM.Utils;

namespace T8MM;

public class App : Application
{
    private const string DOMAIN = "https://api.nexusmods.com";

    private const string API_KEY = "w0nX2W12v7eSd9Q2VbDqOFPacvl0vRYkduur+/8l4+DEYvs=--5dDZNKVaYSndbkP5--aCMUTPQjI/UolxlVL6uCdQ==";
    
    public static IServiceProvider? Provider;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        Provider = ConfigureServices();
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        
        
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var splashScreenViewModel = new SplashScreenViewMode();
            var splashScreen = new SplashScreenView() { DataContext = splashScreenViewModel };
            desktop.MainWindow = splashScreen;
            splashScreen.Show();

            //TODO: Create a queue?
            try
            {
                splashScreenViewModel.StartupMessage = Localization.Resources.KEY_STARTUP_LOAD_USER_SETTINGS;
                await Task.Run(() =>
                {
                    var language = Provider.GetService<IAppSettingService>().AppSettings.Language;
                    Debug.Log($"Settings Language {language}");
                    Localization.Resources.Culture = new CultureInfo(language);
                    
                    splashScreenViewModel.ProgressValue = 25;
                    
                }, splashScreenViewModel.CancellationToken);
                
                splashScreenViewModel.StartupMessage = Localization.Resources.KEY_STARTUP_VERIFY_USER_API;
                await Task.Run(() =>
                {
                    var apikey = Provider.GetService<IAppSettingService>().AppSettings.UserApiKey;
                    if (!string.IsNullOrEmpty(apikey))
                    {
                        Provider.GetService<IProtocolService>().Authenticate(apikey);
                    }
                    splashScreenViewModel.ProgressValue = 100;
                }, splashScreenViewModel.CancellationToken);
                
                await Task.Delay(1500, splashScreenViewModel.CancellationToken);
            }   
            catch (Exception e)
            {
                splashScreen.Close();
                return;
            }
            
            var viewLocalor = Provider?.GetRequiredService<IDataTemplate>();
            var mainViewModel = Provider?.GetRequiredService<T8MMViewModel>();

            desktop.MainWindow = viewLocalor?.Build(mainViewModel) as Window;
            desktop.MainWindow.Show();
            
            splashScreen.Close();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static ServiceProvider ConfigureServices()
    {
        var viewLocator = Current?.DataTemplates.First(x => x is ViewLocator);
        var services = new ServiceCollection();

        services.AddSingleton<IAppSettingService, AppSettingService>();
        
        // Protocol
        services.AddHttpClient<IProtocolService, ProtocolService>(httpClient => httpClient.BaseAddress = new Uri(DOMAIN));
        
        // Services
        if (viewLocator is not null)
            services.AddSingleton(viewLocator);
        services.AddSingleton<PageNavigationService>();

        // ViewModels
        services.AddSingleton<T8MMViewModel>();
        var types =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => !p.IsAbstract && typeof(PageBase).IsAssignableFrom(p));

        foreach (var type in types)
            services.AddSingleton(typeof(PageBase), type);

        return services.BuildServiceProvider();
    }
}