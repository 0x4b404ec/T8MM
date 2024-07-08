    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System.Net.Http;
using System.Threading.Tasks;

namespace T8MM.Services;

public interface IProtocolService
{
    public bool Initialized { get; set; }
    public bool IsValidatedUser { get; set; }
    Task<UserInfoResult?> Authenticate(string apikey);
    Task<ModInfoResult?> RetrieveSpecifiedMod(string gameDomainName, int modId);
}


public partial class ProtocolService(HttpClient? client) : IProtocolService
{
    public bool Initialized { get; set; }
    public bool IsValidatedUser { get; set; }

    private const string DOMAIN = "https://api.nexusmods.com";
}