using PlaylistManager.Data.ToPlaylistManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaylistManager.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;

        public PlaylistService(UserService userService)
        {
            _httpClient = new();
            _userService = userService;
        }

        public List<Playlist> GetMyPlaylists(string token)
        {
            if (token.Length != 172 && !token.StartsWith("Bearer ")) throw new Exception("Unauthorized");

            string userId;
            try
            {
                userId = _userService.GetMe(token).Id;
            }
            catch
            {
                throw;
            }

            List<Data.FromSpotify.Playlist> items = new();
            const int limit = 50;
            int offset = 0;
            int total;
            do
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                HttpResponseMessage response = _httpClient.GetAsync($"https://api.spotify.com/v1/me/playlists?limit={limit}&offset={offset}").Result;
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

                Data.FromSpotify.PlaylistPaginator? page = JsonSerializer.Deserialize<Data.FromSpotify.PlaylistPaginator>(response.Content.ReadAsStream());
                total = page is not null ? page.total : throw new Exception("Generic error.");
                items.AddRange(page.items);
                offset += limit;
            } while (offset < total);
            return items.Count > 0 ? items.Select(x => new Playlist(x, userId)).Where(x => x.IsMine).ToList() : throw new Exception("Generic error.");
        }

        public Playlist? GetQueue(string token)
        {
            return GetMyPlaylists(token).FirstOrDefault(x => x.Name == "Queue");
        }

        public Playlist? GetPlaylist(string token, string playlistId)
        {
            return GetMyPlaylists(token).FirstOrDefault(x => x.Id == playlistId);
        }

        public bool AddTrack(string token, string playlistId, string trackUri)
        {
            throw new NotImplementedException();
        }

        public bool RemoveTrack(string token, string playlistId, string trackUri)
        {
            throw new NotImplementedException();
        }

        public Playlist LoadTracks(string token, Playlist playlist)
        {
            if (token.Length != 172 && !token.StartsWith("Bearer ")) throw new Exception("Unauthorized");

            const int limit = 100;
            int offset = 0;
            int total;
            do
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
                HttpResponseMessage response = _httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlist.Id}/tracks?fields=total,items(track(name,id,external_urls,artists(name,id,external_urls),album(name,id,external_urls,images)))&limit={limit}&offset={offset}").Result;
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                if (!response.IsSuccessStatusCode) throw new Exception(response.ReasonPhrase);

                Data.FromSpotify.TracksPaginator? page = JsonSerializer.Deserialize<Data.FromSpotify.TracksPaginator>(response.Content.ReadAsStream());
                total = page is not null ? page.total : throw new Exception("Generic error.");
                playlist.Tracks.AddRange(page.items.Select(x => new Track(x.track)).Where(x => x.Id is not ""));
                offset += limit;
            } while (offset < total);
            if (playlist.Tracks.Count == 0) throw new Exception("Generic error.");
            return playlist;
        }

        //public bool CreateQueue(string token)
        //{
        //    if (token.Length != 172 && !token.StartsWith("Bearer ")) throw new Exception("Invalid token");

        //    string userId;
        //    try
        //    {
        //        userId = _userService.GetMe(token).Id;
        //    }
        //    catch
        //    {
        //        throw;
        //    }


        //}
    }
}
