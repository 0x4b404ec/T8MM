using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Collections;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using T8MM.Pages;
using T8MM.Services;
using T8MM.Utils;

namespace T8MM;

public partial class T8MMViewModel : ObservableObject
{
    public IAvaloniaReadOnlyList<PageBase> Pages { get; }
    

    [ObservableProperty] private ThemeVariant m_baseTheme;
    [ObservableProperty] private PageBase? m_activePage;
    

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
    }

    [RelayCommand]
    private static void OpenUrl(string url) => UrlUtils.OpenUrl(url);
}