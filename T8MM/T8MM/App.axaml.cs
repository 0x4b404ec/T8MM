using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using T8MM.Pages;
using T8MM.Services;

namespace T8MM;

public partial class App : Application
{
    private IServiceProvider? m_provider;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        m_provider = ConfigureServices();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewLocalor = m_provider?.GetRequiredService<IDataTemplate>();
            var mainViewModel = m_provider?.GetRequiredService<T8MMViewModel>();
            
            desktop.MainWindow = viewLocalor?.Build(mainViewModel) as Window;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static ServiceProvider ConfigureServices()
    {
        var viewLocator = Current?.DataTemplates.First(x => x is ViewLocator);
        var services = new ServiceCollection();

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