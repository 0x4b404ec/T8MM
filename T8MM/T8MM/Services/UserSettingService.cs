    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using T8MM.Global;
using T8MM.Utils;

namespace T8MM.Services;

public interface IUserSettingService
{
    public UserSettingDataModel Settings { get; }
    public string? UserApiKey { get; }
    public string? UserLanguage { get; }
    public string? GameRootPath { get; }
}

public class UserSettingService : IUserSettingService
{
    public UserSettingDataModel Settings { get; private set; }

    public string UserApiKey
    {
        get =>Settings.UserApiKey;
        set => Settings.UserApiKey = value;
    }

    public string UserLanguage
    {
        get => Settings.UserLanguage;
        set => Settings.UserLanguage = value;
    }

    public string GameRootPath
    {
        get => Settings.GameRootPath;
        set => Settings.GameRootPath = value;
    }
    
    public UserSettingService()
    {
        ReadUserSetting();
    }

    public void ReadUserSetting()
    {
        Settings = FileUtils.ReadDecryptJsonObjectInFile<UserSettingDataModel>(Constants.UserSettingFilePath);
    }

    public void WriteUserSetting()
    {
        
    }
}

public class UserSettingDataModel
{
    public string UserApiKey;
    public string UserLanguage;
    public string GameRootPath;
}