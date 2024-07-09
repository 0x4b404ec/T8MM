    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using T8MM.Utils;

namespace T8MM.Services;


public partial class ProtocolService
{ 
    /// <summary>
    /// Retrieve specified mod, from a specified game. Cached for 5 minutes.
    /// See https://app.swaggerhub.com/apis-docs/NexusMods/nexus-mods_public_api_params_in_form_data/1.0#/Mods/get_v1_games_game_domain_name_mods_id.json
    /// </summary>
    private const string RETRIEVE_SPECIFIED_MOD_REQUEST = "v1/games/{0}/mods/{1}.json";
    
    public async Task<ModInfoResult?> RetrieveSpecifiedMod(string gameDomainName, int modId)
    {
        var suburl = string.Format(RETRIEVE_SPECIFIED_MOD_REQUEST, gameDomainName, modId);
        
        Debug.Assert(Initialized, $"Protocol not initialized!");
        
        var response = await m_client.GetAsync(suburl);

        var content = await response.Content.ReadAsStringAsync();
        
        return response.IsSuccessStatusCode
            ? JsonConvert.DeserializeObject<ModInfoResult>(content)
            : null;
    }
}

public partial class ModInfoResult : ObservableObject
{
    #region response fields
    
    [JsonProperty("uid"), ObservableProperty] private int m_uid;
    [JsonProperty("mod_id"), ObservableProperty] private int m_modId;
    [JsonProperty("author"), ObservableProperty] private int m_author;
    [JsonProperty("uploaded_users_profile_url"), ObservableProperty] private int m_uploadedUsersProfileUrl;
    [JsonProperty("domain_name"), ObservableProperty] private int m_domainName;
    [JsonProperty("game_id"), ObservableProperty] private int m_gameId;
    [JsonProperty("name"), ObservableProperty] private string m_name;
    [JsonProperty("summary"), ObservableProperty] private string m_summary;
    [JsonProperty("description"), ObservableProperty] private string m_description;
    [JsonProperty("picture_url"), ObservableProperty] private string m_pictureUrl;
    [JsonProperty("mod_downloads"), ObservableProperty] private string m_modDownloads;
    [JsonProperty("mod_unique_downloads"), ObservableProperty] private string m_modUniqueDownloads;
    [JsonProperty("category_id"), ObservableProperty] private string m_categoryId;
    [JsonProperty("version"), ObservableProperty] private string m_version;
    
    #endregion

    #region local cache

    [ObservableProperty] private bool m_isDataFromWeb;

    #endregion
}