using PlaylistManager.Data.ToPlaylistManager;
using System.Text.Json;

namespace PlaylistManager.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUtils _utils;
        private readonly IUserService _userService;
        private readonly ITrackService _trackService;
        public enum ContainsTrack
        {
            No,
            Maybe,
            Yes
        }

        public PlaylistService(Utils utils, UserService userService, TrackService trackService)
        {
            _utils = utils;
            _userService = userService;
            _trackService = trackService;
        }

        public List<Playlist> GetMyPlaylists(string token)
        {
            string userId = _userService.GetMe(token).Id;
            List<Playlist> playlists = new();
            int offset = 0;
            HttpClient httpClient = _utils.HttpClient(token);
            HttpResponseMessage response = httpClient.GetAsync($"https://api.spotify.com/v1/me/playlists?limit={1}&offset={0}").Result;
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.PlaylistPaginator? page = JsonSerializer.Deserialize<Data.FromSpotify.PlaylistPaginator>(response.Content.ReadAsStream());
            int total = page is not null ? page.total : throw new Exception("Generic error.");
            List<Task> tasks = new();

            do
            {
                tasks.Add(GetMyPlaylistsPage(httpClient, playlists, offset, userId));
            } while ((offset += 100) < total);
            Task.WaitAll(tasks.ToArray());
            return playlists.Count > 0 ? playlists : throw new Exception("Generic error.");
        }

        private async Task GetMyPlaylistsPage(HttpClient httpClient, List<Playlist> playlists, int offset, string userId)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.spotify.com/v1/me/playlists?limit={50}&offset={offset}");
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.PlaylistPaginator? page = JsonSerializer.Deserialize<Data.FromSpotify.PlaylistPaginator>(response.Content.ReadAsStream()) ?? throw new Exception("500");
            playlists.AddRange(page.items.Select(x => new Playlist(x, userId)).Where(x => (bool)x.IsMine! || x.IsCollaborative));
        }

        public Playlist GetPlaylist(string token, string playlistId)
        {
            return GetMyPlaylists(token).FirstOrDefault(x => playlistId != "pmqueue" ? x.Id == playlistId : x.Name == "Queue - PM" && (bool)x.IsMine! && !x.IsCollaborative) ?? throw new Exception("404");
        }

        public void AddTrack(string token, string playlistId, string trackId)
        {
            HttpClient httpClient = _utils.HttpClient(token);
            if (playlistId == "pmqueue") playlistId = GetPlaylist(token, playlistId).Id;
            HttpRequestMessage request = new(HttpMethod.Post, $"https://api.spotify.com/v1/playlists/{playlistId}/tracks");
            request.Content = new StringContent($"[\"spotify:track:{trackId}\"]");
            HttpResponseMessage response = httpClient.SendAsync(request).Result;
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
        }

        public void RemoveTrack(string token, string playlistId, string trackId)
        {
            HttpClient httpClient = _utils.HttpClient(token);
            if (playlistId == "pmqueue") playlistId = GetPlaylist(token, playlistId).Id;
            HttpRequestMessage request = new(HttpMethod.Delete, $"https://api.spotify.com/v1/playlists/{playlistId}/tracks");
            request.Content = new StringContent($"{{\"tracks\":[{{\"uri\":\"spotify:track:{trackId}\"}}]}}");
            HttpResponseMessage response = httpClient.SendAsync(request).Result;
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
        }

        public ContainsTrack CheckTrack(string token, string playlistId, string trackId)
        {
            Track track = _trackService.GetTrack(token, trackId);
            Playlist playlist = GetPlaylist(token, playlistId);
            int offset = 0;
            int total = playlist.TracksNumber;
            List<Task<ContainsTrack>> tasks = new();
            HttpClient httpClient = _utils.HttpClient(token);
            do
            {
                tasks.Add(CheckTrackPage(httpClient, playlist.Id, track, offset));
            } while ((offset += 100) < total);
            Task.WaitAll(tasks.ToArray());

            if (tasks.Any(x => x.Result == ContainsTrack.Yes)) return ContainsTrack.Yes;
            if (tasks.Any(x => x.Result == ContainsTrack.Maybe)) return ContainsTrack.Maybe;
            return ContainsTrack.No;

        }

        private async Task<ContainsTrack> CheckTrackPage(HttpClient httpClient, string playlistId, Track track, int offset)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlistId}/tracks?fields=total,items(track(name,id,external_urls,artists(name,id,external_urls),album(name,id,external_urls,images)))&limit={100}&offset={offset}");
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.TracksPaginator? page = JsonSerializer.Deserialize<Data.FromSpotify.TracksPaginator>(response.Content.ReadAsStream()) ?? throw new Exception("Generic error.");
            if (page.items.Any(x => x.track.id == track.Id)) return ContainsTrack.Yes;
            if (page.items.Any(x => x.track.name.ToLower() == track.Name.ToLower() && x.track.artists.Any(y => track.Artists.Any(z => z.Name.ToLower() == y.name.ToLower())))) return ContainsTrack.Maybe;
            return ContainsTrack.No;
        }

        public void CreateQueue(string token)
        {
            HttpClient httpClient = _utils.HttpClient(token);
            HttpRequestMessage request = new(HttpMethod.Post, $"https://api.spotify.com/v1/me/playlists");
            request.Content = new StringContent($"{{\"name\":\"Queue - PM\",\"public\":false}}"/*\"description\":\"Created by PlaylistManager\",*/);
            HttpResponseMessage response = httpClient.SendAsync(request).Result;
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
        }

        public Playlist LoadTracks(string token, Playlist playlist)
        {

            int offset = 0;
            int total = playlist.TracksNumber;
            List<Task> tasks = new();
            HttpClient httpClient = _utils.HttpClient(token);
            do
            {
                tasks.Add(LoadTracksPage(httpClient, playlist, offset));
            } while ((offset += 100) < total);
            Task.WaitAll(tasks.ToArray());
            if (playlist.Tracks.Count == 0) throw new Exception("Generic error.");
            return playlist;
        }

        private async Task LoadTracksPage(HttpClient httpClient, Playlist playlist, int offset)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlist.Id}/tracks?fields=total,items(track(name,id,external_urls,artists(name,id,external_urls),album(name,id,external_urls,images)))&limit={100}&offset={offset}");
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.TracksPaginator? page = JsonSerializer.Deserialize<Data.FromSpotify.TracksPaginator>(response.Content.ReadAsStream()) ?? throw new Exception("Generic error.");
            playlist.Tracks.AddRange(page.items.Select(x => new Track(x.track)).Where(x => x.Id is not ""));
        }

        public List<Playlist> GetPlaylists(string token, List<Tuple<string, long>> ids)
        {
            List<Playlist> playlists = new();
            if (ids.Count == 0) return playlists;
            int offset = 0;
            List<Task> tasks = new();
            HttpClient httpClient = _utils.HttpClient(token);
            do
            {
                tasks.Add(GetPlaylistsPage(httpClient, ids.GetRange(offset, offset + 1 > ids.Count - offset ? ids.Count - offset : offset + 1), playlists));
            } while ((offset += 1) < ids.Count);
            Task.WaitAll(tasks.ToArray());
            return playlists;
        }

        private async Task GetPlaylistsPage(HttpClient httpClient, List<Tuple<string, long>> ids, List<Playlist> playlists)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{string.Join(',', ids.Select(x => x.Item1))}");
            if ((int)response.StatusCode == 404) return;
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.Playlist _playlists = JsonSerializer.Deserialize<Data.FromSpotify.Playlist>(response.Content.ReadAsStream()) ?? throw new Exception("500");
            playlists.Add(new Playlist(_playlists, null, ids.FirstOrDefault(x => x.Item1 == _playlists.id)?.Item2));
        }
    }
}
