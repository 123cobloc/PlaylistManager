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

        public List<Track> GetTracks(string token, List<Tuple<string, long>> ids)
        {
            List<Track> tracks = new();
            if (ids.Count == 0) return tracks;
            int offset = 0;
            List<Task> tasks = new();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            do
            {
                tasks.Add(GetTracksPage(ids.GetRange(offset, offset + 50 > ids.Count - offset ? ids.Count - offset : offset + 50), tracks));
            } while ((offset += 50) < ids.Count);
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            Task.WaitAll(tasks.ToArray());
            return tracks;
        }

        private async Task GetTracksPage(List<Tuple<string, long>> ids, List<Track> tracks)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://api.spotify.com/v1/tracks?ids={string.Join(',', ids.Select(x => x.Item1))}");
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.GetTracks _tracks = JsonSerializer.Deserialize<Data.FromSpotify.GetTracks>(response.Content.ReadAsStream()) ?? throw new Exception("500");
            tracks.AddRange(_tracks.tracks.Where(x => x is not null).Select(x => new Track(x, ids.FirstOrDefault(y => y.Item1 == x.id)?.Item2)));
        }
    }
}
