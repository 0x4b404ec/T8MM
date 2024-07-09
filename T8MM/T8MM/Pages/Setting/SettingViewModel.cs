    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using CommunityToolkit.Mvvm.DependencyInjection;
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
    
    
    [ObservableProperty] private string m_userApiKey;
    [ObservableProperty] private SolidColorBrush m_fontColor;
    [ObservableProperty] private bool m_hasUserApiKey;

    [ObservableProperty] private Language m_selectedLanguage;

    [ObservableProperty] private string m_gameRootFolder;

    [ObservableProperty] private bool m_hasParamChanged;
    
    private IAppSettingService m_appSettingService;
    private IProtocolService m_protocolService;
    
    public SettingViewModel() : base("Settings", MaterialIconKind.Settings)
    {
        m_appSettingService = Ioc.Default.GetRequiredService<IAppSettingService>();
        m_protocolService = Ioc.Default.GetRequiredService<IProtocolService>();
        
        if (m_appSettingService != null)
        {
            Debug.Log($"AppSettings: {m_appSettingService.ToString()}");
            SelectedLanguage =
                SupportLanguages.FirstOrDefault(l => l.Code.Equals(m_appSettingService.AppSettings.Language)) ??
                SupportLanguages[1];
            UserApiKey = m_appSettingService.AppSettings.UserApiKey;
            Debug.Log($"UserApiKey {Ioc.Default.GetRequiredService<IProtocolService>().IsValidatedUser}\n {UserApiKey}");
            HasUserApiKey = !string.IsNullOrEmpty(m_userApiKey);
            GameRootFolder = m_appSettingService.AppSettings.GamePath;
            FontColor = Ioc.Default.GetRequiredService<IProtocolService>().IsValidatedUser ? new SolidColorBrush(Colors.LightGreen) : new SolidColorBrush(Colors.OrangeRed);
        }
    }

    partial void OnSelectedLanguageChanged(Language value)
    {
        Debug.Log($"OnSelectedLanguageChanged {value.DisplayName} {value.Code}");
        CheckHasParamChanged();
    }

    partial void OnUserApiKeyChanged(string? value)
    {
        Debug.Log($"OnUserApiKeyChanged {value}");
        CheckHasParamChanged();
        // TODO : Check with protocol
        HasUserApiKey = !string.IsNullOrEmpty(UserApiKey);
    }
    
    partial void OnGameRootFolderChanged(string? value)
    {
        Debug.Log($"OnGameRootFolderChanged {value}");
        CheckHasParamChanged();
    }

    private void CheckHasParamChanged()
    {
         HasParamChanged = m_appSettingService.AppSettings.Language != SelectedLanguage.Code ||
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
        m_appSettingService.AppSettings.Language = SelectedLanguage.Code;
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