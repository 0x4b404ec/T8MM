    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using Newtonsoft.Json;
using T8MM.Global;
using T8MM.Models;
using T8MM.Utils;

namespace T8MM.Services;

public interface IAppSettingService
{
    public AppSettingDataModel AppSettings { get; }

    public void Save();

    public void Load();

    public string ToString();
}

public class AppSettingService : IAppSettingService
{
    public AppSettingDataModel AppSettings { get; private set; }
    public void Save()
    {
        FileUtils.WriteEncryptJsonObjectInFile(Constants.UserSettingFilePath, AppSettings);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(AppSettings);
    }

    public void Load()
    {
        AppSettings =  FileUtils.ReadDecryptJsonObjectInFile<AppSettingDataModel>(Constants.UserSettingFilePath);
        if (AppSettings is null)
        {
            AppSettings = new AppSettingDataModel();
            Save();
        }
    }

    public AppSettingService()
    {
        Load();
    }
    
}