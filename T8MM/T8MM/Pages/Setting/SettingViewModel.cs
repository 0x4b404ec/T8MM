    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;
using T8MM.Services;
using T8MM.Utils;

namespace T8MM.Pages.Setting;

public partial class Language(string displayName, string code) : ObservableObject
{
    [ObservableProperty] private string m_displayName = displayName;
    [ObservableProperty] private string m_code = code;
}

public partial class SettingViewModel : PageBase
{
    public ObservableCollection<Language> SupportLanguages { get; } =
    [
        new Language("中文(简体)", "zh-hans"),
        new Language("English(US)", "en-us")
    ];
    
    [ObservableProperty] private string? m_userApiKey;
    [ObservableProperty] private SolidColorBrush m_fontColor;
    [ObservableProperty] private bool m_hasUserApiKey;
    
    [ObservableProperty] private int? m_selectedLanguageIndex;

    [ObservableProperty] private string? m_gameRootFolder;

    [ObservableProperty] private bool m_hasParamChanged;
    
    private IAppSettingService m_appSettingService;
    private IProtocolService m_protocolService;
    
    public SettingViewModel() : base("Settings", MaterialIconKind.Settings)
    {
        m_appSettingService = App.Provider.GetService<IAppSettingService>();
        m_protocolService = App.Provider.GetService<IProtocolService>();
        
        if (m_appSettingService != null)
        {
            SelectedLanguageIndex = m_appSettingService.AppSettings.Language;
            UserApiKey = m_appSettingService.AppSettings.UserApiKey;
            HasUserApiKey = !string.IsNullOrEmpty(m_userApiKey);
            GameRootFolder = m_appSettingService.AppSettings.GamePath;
            // FontColor = m_protocolService.IsValidatedUser ?  : 
        }
    }

    partial void OnSelectedLanguageIndexChanged(int? value)
    {
        Debug.Log($"OnSelectedLanguageItemChanged {value}");
        CheckHasParamChanged();
    }

    partial void OnUserApiKeyChanged(string? value)
    {
        Debug.Log($"OnUserApiKeyChanged {value}");
        CheckHasParamChanged();
        // TODO : Check with protocol
        HasUserApiKey = !string.IsNullOrEmpty(m_userApiKey);
    }
    
    partial void OnGameRootFolderChanged(string? value)
    {
        Debug.Log($"OnGameRootFolderChanged {value}");
        CheckHasParamChanged();
    }

    private void CheckHasParamChanged()
    {
         HasParamChanged = m_appSettingService.AppSettings.Language != SelectedLanguageIndex ||
                           m_appSettingService.AppSettings.GamePath != GameRootFolder ||
                           m_appSettingService.AppSettings.UserApiKey != UserApiKey;
    }

    [RelayCommand]
    private void CheckUserApi()
    {
        
    }
    
    [RelayCommand]
    private void SaveButtonClicked()
    {
        m_appSettingService.AppSettings.Language = SelectedLanguageIndex.Value;
        m_appSettingService.AppSettings.GamePath = GameRootFolder;
        m_appSettingService.AppSettings.UserApiKey = UserApiKey;
        m_appSettingService.Save();
        CheckHasParamChanged();
    }

    [RelayCommand]
    private async Task FindTekken8Exe()
    {
        var topLevel = TopLevel.GetTopLevel(((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow);
        IReadOnlyList<IStorageFolder> files = await topLevel?.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { AllowMultiple = false })!;
        GameRootFolder = files[0].Path.LocalPath;
    }
}