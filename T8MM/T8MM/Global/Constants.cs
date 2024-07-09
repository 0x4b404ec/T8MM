    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System;
using System.IO;

namespace T8MM.Global;

public static class Constants
{
    private const string MOD_CACHE_FILE_NAME = "ModCache.json";
    public static readonly string ModsFolder = $"{AppDomain.CurrentDomain.BaseDirectory}/mods".Replace("//","/");
    public static string ModCacheFilePath = Path.Combine(ModsFolder, MOD_CACHE_FILE_NAME);
    
    public const string APP_SETTING_FILE_NAME = "appsettings.json";
    public static readonly string UserSettingFolder = $"{AppDomain.CurrentDomain.BaseDirectory}/user".Replace("//","/");
    public static string UserSettingFilePath = Path.Combine(UserSettingFolder, APP_SETTING_FILE_NAME);
}
