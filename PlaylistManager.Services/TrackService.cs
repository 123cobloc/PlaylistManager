using PlaylistManager.Data.ToPlaylistManager;
using System.Text.Json;

namespace PlaylistManager.Services
{
    public class TrackService : ITrackService
    {
        private readonly IUtils _utils;

        public TrackService(Utils utils)
        {
            _utils = utils;
        }

        public Track GetTrack(string token, string trackId)
        {
            HttpClient httpClient = _utils.HttpClient(token);
            HttpResponseMessage response = httpClient.GetAsync($"https://api.spotify.com/v1/tracks/{trackId}").Result;
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
            HttpClient httpClient = _utils.HttpClient(token);
            do
            {
                tasks.Add(GetTracksPage(httpClient, ids.GetRange(offset, offset + 50 > ids.Count - offset ? ids.Count - offset : offset + 50), tracks));
            } while ((offset += 50) < ids.Count);
            Task.WaitAll(tasks.ToArray());
            return tracks;
        }

        private async Task GetTracksPage(HttpClient httpClient, List<Tuple<string, long>> ids, List<Track> tracks)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.spotify.com/v1/tracks?ids={string.Join(',', ids.Select(x => x.Item1))}");
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.GetTracks _tracks = JsonSerializer.Deserialize<Data.FromSpotify.GetTracks>(response.Content.ReadAsStream()) ?? throw new Exception("500");
            tracks.AddRange(_tracks.tracks.Where(x => x is not null).Select(x => new Track(x, ids.FirstOrDefault(y => y.Item1 == x.id)?.Item2)));
        }
    }
}
