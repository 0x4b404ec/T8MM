    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;

namespace T8MM.Pages.Setting;

public partial class SettingViewModel : PageBase
{
    private AvaloniaDictionary<string, string> SupportLanguages { get; } = new()
    {
        { "中文(简体)", "zh-hans" },
        { "English(US)", "en-us" }
    };
    
    [ObservableProperty]
    private string? m_selectedLanguageItem;

    public SettingViewModel(): base("Settings", MaterialIconKind.Settings)
    {
        
    }
}