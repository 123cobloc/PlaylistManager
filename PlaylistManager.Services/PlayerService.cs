using PlaylistManager.Data.ToPlaylistManager;
using System.Net;
using System.Text.Json;

namespace PlaylistManager.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUtils _utils;
        private readonly IPlaylistService _playlistService;
        public PlayerService(Utils utils, PlaylistService playlistService)
        {
            _utils = utils;
            _playlistService = playlistService;
        }

        public Track GetCurrentTrack(string token)
        {
            HttpClient httpClient = _utils.HttpClient(token);
            HttpResponseMessage response = httpClient.GetAsync($"https://api.spotify.com/v1/me/player").Result;
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            if (response.Content.Headers.ContentLength == 0) throw new Exception("204");
            Data.FromSpotify.Player? player = JsonSerializer.Deserialize<Data.FromSpotify.Player>(response.Content.ReadAsStream());
            Track track = player?.item is not null ? new Track(player.item) : throw new Exception(player is null ? "500" : "204");
            track.IsFromQueue = _playlistService.CheckTrack(token, "pmqueue", track.Id) == PlaylistService.ContainsTrack.Yes;
            return track;
        }
    }
}
