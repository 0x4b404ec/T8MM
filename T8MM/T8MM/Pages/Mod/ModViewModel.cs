/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using T8MM.Global;
using T8MM.Services;
using T8MM.Utils;

namespace T8MM.Pages.Mod;

public partial class ModViewModel : PageBase
{
    public ObservableCollection<ModInfoResult?> Mods { get; } = new();
    
    [ObservableProperty]
    private bool m_isBusy;

    private readonly IProtocolService m_protocolService;
    
    public ModViewModel(IProtocolService protocolService) : base("Mods", MaterialIconKind.ModeEdit)
    {
        m_protocolService = protocolService;
        _ = ModRefresh();
    }
    
    [RelayCommand]
    private async Task ModRefresh()
    {
        Debug.Log($"{this} ModRefresh. {Constants.ModsFolder}");
        
        if (IsBusy) return;
        IsBusy = true;
        
        FileUtils.CreateDirectoryIfNotExists(Constants.ModsFolder);
        var modFiles = FileUtils.GetFileNameListInPath(Constants.ModsFolder, "*.rar");
        // var modFiles = FileUtils.GetFileNameListInPath(ModUtils.ModsFolder, "*.rar|*.zip");
        
        Mods.Clear();
        foreach (var file in modFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
           
            Debug.Log($"file name : {fileName}");
            
            if (ModUtils.ModPackageNameValid(fileName))
            {
                var infoFilePath = $"{Constants.ModsFolder}/{fileName}_Info.json";
                
                var info = FileUtils.ReadDecryptJsonObjectInFile<ModInfoResult>(infoFilePath);
                
                Debug.Log($"Get mod file {infoFilePath}");
                
                if (m_protocolService.IsValidatedUser)
                {
                    if (info is null || !info.IsDataFromWeb)
                    {
                        //TODO: Display an error tips if response failed.
                        info = GetInfoFromWeb(fileName).Result;
                        Debug.Log($"Get info from web {info}");
                        FileUtils.WriteEncryptJsonObjectInFile(infoFilePath, info);
                    }
                    Mods.Add(info);
                }
                else
                {
                    Mods.Add(info ?? GetInfoFromLocal(fileName));
                }
            }
        }
        IsBusy = false;
    }

    private ModInfoResult GetInfoFromLocal(string fileName)
    {
        ModUtils.GetBasicInfoByPackage(fileName, out string modName, out int id, out string version, out string updateTime);
        return new ModInfoResult() {Name = modName, ModId = id, Version = version};
    }

    private async Task<ModInfoResult?> GetInfoFromWeb(string fileName)
    {
        ModInfoResult? ret = new();
        ModUtils.GetBasicInfoByPackage(fileName, out string modName, out int id, out string version, out string updateTime);

        var result = await m_protocolService.RetrieveSpecifiedMod("tekken8", id);

        if (result is not null)
        {
            ret = result;
            ret.IsDataFromWeb = true;
        }
        else
        {
            ret.Name = modName;
            ret.ModId = id;
            ret.Version = version;
        }

        return ret;
    }
}