using PlaylistManager.Data.ToPlaylistManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaylistManager.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IUtils _utils;
        private readonly HttpClient _httpClient;
        public AlbumService(Utils utils)
        {
            _utils = utils;
            _httpClient = new();
        }

        public List<Album> GetAlbums(string token, List<Tuple<string, long>> ids)
        {
            List<Album> albums = new();
            if (ids.Count == 0) return albums;
            int offset = 0;
            List<Task> tasks = new();
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            do
            {
                tasks.Add(GetAlbumsPage(ids.GetRange(offset, offset + 20 > ids.Count - offset ? ids.Count - offset : offset + 20), albums));
            } while ((offset += 20) < ids.Count);
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            Task.WaitAll(tasks.ToArray());
            return albums;
        }

        private async Task GetAlbumsPage(List<Tuple<string, long>> ids, List<Album> albums)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://api.spotify.com/v1/albums?ids={string.Join(",", ids.Select(x => x.Item1))}");
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.GetAlbums _albums = JsonSerializer.Deserialize<Data.FromSpotify.GetAlbums>(response.Content.ReadAsStream()) ?? throw new Exception("500");
            albums.AddRange(_albums.albums.Where(x => x is not null).Select(x => new Album(x, ids.FirstOrDefault(y => y.Item1 == x.id)?.Item2)));
        }
    }
}
