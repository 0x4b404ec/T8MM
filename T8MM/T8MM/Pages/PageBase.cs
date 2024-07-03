    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;

namespace T8MM.Pages;

public abstract partial class PageBase(string displayName, MaterialIconKind icon, int index = 0) : ObservableValidator
{
    [ObservableProperty] private string m_displayName = displayName;
    [ObservableProperty] private MaterialIconKind m_icon = icon;
    [ObservableProperty] private int m_index = index;
}