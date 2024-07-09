    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace T8MM.Services;

public interface IProtocolService
{
    void Initialize();
    bool IsValidatedUser { get; set; }
    Task<UserInfoResult?> Authenticate(string apikey);
    Task<ModInfoResult?> RetrieveSpecifiedMod(string gameDomainName, int modId);
}


public partial class ProtocolService : IProtocolService
{
    private const string DOMAIN = "https://api.nexusmods.com";
    
    private HttpClient m_client;

    public ProtocolService()
    {
    }

    public void Initialize()
    {
        m_client = new HttpClient();
        m_client.BaseAddress = new Uri(DOMAIN);
        Initialized = true;
    }

    public bool IsValidatedUser { get; set; }
    public bool Initialized { get; set; }
}