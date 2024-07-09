    
/****************************************************************
 *          Copyright (c) 2024 0x4b404ec MIT License            *
 *                                                              *
 *          0x4b404ec (https://github.com/0x4b404ec)            *
 ****************************************************************/

using System.Threading.Tasks;

namespace T8MM.Services;


public partial class ProtocolService
{ 
    /// <summary>
    /// Returns specified game, along with download count, file count and categories.
    /// See https://app.swaggerhub.com/apis-docs/NexusMods/nexus-mods_public_api_params_in_form_data/1.0#/Games/get_v1_games_game_domain.json
    /// </summary>
    private const string RETRIEVE_SPECIFIED_GAME_REQUEST = "v1/games/{0}}.json";
    
    
}