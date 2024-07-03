using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SukiUI;
using SukiUI.Controls;
using SukiUI.Models;
using T8MM.Pages;
using T8MM.Services;
using T8MM.Utils;

namespace T8MM;

public partial class T8MMViewModel : ObservableObject
{
    public IAvaloniaReadOnlyList<PageBase> Pages { get; }
    
    public IAvaloniaReadOnlyList<SukiColorTheme> Themes { get; }

    [ObservableProperty] private ThemeVariant m_baseTheme;
    [ObservableProperty] private PageBase? m_activePage;


    private readonly SukiTheme m_theme;

    public T8MMViewModel(IEnumerable<PageBase> pages, PageNavigationService pageNavigationService)
    {
        #region pages

        Pages = new AvaloniaList<PageBase>(pages.OrderBy(x => x.Index).ThenBy(x => x.DisplayName));

        pageNavigationService.NavigationRequested += pageType =>
        {
            var page = Pages.FirstOrDefault(x => x.GetType() == pageType);
            if (page is null || ActivePage?.GetType() == pageType) return;
            ActivePage = page;
        };

        #endregion
        
        #region theme

        m_theme = SukiTheme.GetInstance();
        Themes = m_theme.ColorThemes;
        BaseTheme = m_theme.ActiveBaseTheme;

        m_theme.OnBaseThemeChanged += variant =>
        {
            BaseTheme = variant;
            SukiHost.ShowToast($"Successfully Changed Theme", $"Changed Theme To {variant}");
        };

        m_theme.OnColorThemeChanged += theme =>
        {
            SukiHost.ShowToast($"Successfully Changed Color", $"Changed To {theme.DisplayName}");
        };
        
        #endregion
    }

    [RelayCommand]
    private static void OpenUrl(string url) => UrlUtils.OpenUrl(url);
}