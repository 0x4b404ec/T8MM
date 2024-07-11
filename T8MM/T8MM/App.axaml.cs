using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using T8MM.Pages;
using T8MM.Services;

namespace T8MM;

public class App : Application
{
    private const string API_KEY = "w0nX2W12v7eSd9Q2VbDqOFPacvl0vRYkduur+/8l4+DEYvs=--5dDZNKVaYSndbkP5--aCMUTPQjI/UolxlVL6uCdQ==";
    
    private IServiceProvider? m_provider;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override async void OnFrameworkInitializationCompleted()
    {
        m_provider = ConfigureServices();
        Ioc.Default.ConfigureServices(m_provider);
        
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var appSetting = Ioc.Default.GetRequiredService<IAppSettingService>();
            var protocol = Ioc.Default.GetRequiredService<IProtocolService>();
            protocol.Initialize();
            
            var viewLocalor = Ioc.Default.GetRequiredService<IDataTemplate>();
            var mainViewModel = Ioc.Default.GetRequiredService<T8MMViewModel>();

            desktop.MainWindow = viewLocalor.Build(mainViewModel) as Window;
            desktop.MainWindow.Show();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static ServiceProvider ConfigureServices()
    {
        var viewLocator = Current?.DataTemplates.First(x => x is ViewLocator);
        var services = new ServiceCollection();

        services.AddSingleton<IAppSettingService, AppSettingService>();
        
        // Protocol
        services.AddSingleton<IProtocolService, ProtocolService>();
        
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