using PlaylistManager.Data.ToPlaylistManager;
using System.Net;
using System.Text.Json;

namespace PlaylistManager.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUtils _utils;
        private readonly HttpClient _httpClient;
        private readonly IPlaylistService _playlistService;
        public PlayerService(Utils utils, PlaylistService playlistService)
        {
            _utils = utils;
            _httpClient = new();
            _playlistService = playlistService;
        }

        public Track GetCurrentTrack(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            HttpResponseMessage response = _httpClient.GetAsync($"https://api.spotify.com/v1/me/player").Result;
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            if (response.Content.Headers.ContentLength == 0) throw new Exception("204");
            Data.FromSpotify.Player? player = JsonSerializer.Deserialize<Data.FromSpotify.Player>(response.Content.ReadAsStream());
            Track track = player?.item is not null ? new Track(player.item) : throw new Exception(player is null ? "500" : "204");
            if (player?.context?.type == "playlist") track.IsFromQueue = player.context.uri == $"spotify:playlist:{_playlistService.GetPlaylist(token, "pmqueue").Id}";
            return track;
        }
    }
}
