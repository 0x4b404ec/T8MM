    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using T8MM.Utils;

namespace T8MM.Services;


public partial class ProtocolService
{
    /// <summary>
    /// Checks an API key is valid and returns the user's details.
    /// See https://app.swaggerhub.com/apis-docs/NexusMods/nexus-mods_public_api_params_in_form_data/1.0#/User/post_v1_users_validate.json
    /// </summary>
    private const string CHECKS_API_VALID_REQUEST = "v1/users/validate.json";
    
    public async Task<UserInfoResult?> Authenticate(string apikey)
    {
        var response = await client?.GetAsync(CHECKS_API_VALID_REQUEST);

        var content = await response.Content.ReadAsStringAsync();
        
        if (response.IsSuccessStatusCode)
        {
            Debug.Log($"{this} User Validated");
            client.DefaultRequestHeaders.Add("apikey", apikey);
            IsValidatedUser = true;
            return JsonConvert.DeserializeObject<UserInfoResult>(content);
        }
        return null;
    }
}

public partial class UserInfoResult: ObservableObject
{
    [JsonProperty("user_id"), ObservableProperty] private int m_userId;
    [JsonProperty("key"), ObservableProperty] private string m_apiKey;
    [JsonProperty("name"), ObservableProperty] private string m_userName;
    [JsonProperty("is_premium"), ObservableProperty] private bool m_isPremium;
    [JsonProperty("is_supporter"), ObservableProperty] private bool m_isSupporter;
    [JsonProperty("email"), ObservableProperty] private string m_email;
    [JsonProperty("profile_url"), ObservableProperty] private string m_profileUrl;
}