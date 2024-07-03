    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;

namespace T8MM.Pages.Setting;

public partial class Language(string displayName, string code) : ObservableObject
{
    [ObservableProperty] private string m_displayName = displayName;
    [ObservableProperty] private string m_code = code;
}

public partial class SettingViewModel() : PageBase("Settings", MaterialIconKind.Settings)
{
    public ObservableCollection<Language> SupportLanguages { get; } =
    [
        new Language("中文(简体)", "zh-hans"),
        new Language("English(US)", "en-us")
    ];
    
    [ObservableProperty] private string? m_userApiKey;
    
    [ObservableProperty] private string? m_selectedLanguageItem;

    [ObservableProperty] private string? m_gameRootFolder;
    
    
    [RelayCommand]
    private async Task FindTekken8Exe()
    {
        var topLevel = TopLevel.GetTopLevel(((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow);
        IReadOnlyList<IStorageFolder> files = await topLevel?.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions { AllowMultiple = false })!;
        GameRootFolder = files[0].Path.LocalPath;
    }
}