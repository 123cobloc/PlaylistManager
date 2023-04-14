using PlaylistManager.Data.ToPlaylistManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaylistManager.Services
{
    public class TrackService : ITrackService
    {
        private readonly IUtils _utils;
        private readonly HttpClient _httpClient;

        public TrackService(Utils utils)
        {
            _utils = utils;
            _httpClient = new();
        }

        public Track GetTrack(string token, string trackId)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            HttpResponseMessage response = _httpClient.GetAsync($"https://api.spotify.com/v1/tracks/{trackId}").Result;
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));

            Data.FromSpotify.Track track = JsonSerializer.Deserialize<Data.FromSpotify.Track>(response.Content.ReadAsStream()) ?? throw new Exception("Generic error.");
            return new Track(track);
        }
    }
}
