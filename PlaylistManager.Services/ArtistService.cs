﻿using PlaylistManager.Data.ToPlaylistManager;
using System.Text.Json;

namespace PlaylistManager.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IUtils _utils;
        public ArtistService(Utils utils)
        {
            _utils = utils;
        }

        public List<Artist> GetArtists(string token, List<Tuple<string, long>> ids)
        {
            List<Artist> artists = new();
            if (ids.Count == 0) return artists;
            int offset = 0;
            List<Task> tasks = new();
            HttpClient httpClient = _utils.HttpClient(token);
            do
            {
                tasks.Add(GetArtistsPage(httpClient, ids.GetRange(offset, offset + 50 > ids.Count - offset ? ids.Count - offset : offset + 50), artists));
            } while ((offset += 50) < ids.Count);
            Task.WaitAll(tasks.ToArray());
            return artists;
        }

        private async Task GetArtistsPage(HttpClient httpClient, List<Tuple<string, long>> ids, List<Artist> artists)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.spotify.com/v1/artists?ids={string.Join(',', ids.Select(x => x.Item1))}");
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.GetArtists _artists = JsonSerializer.Deserialize<Data.FromSpotify.GetArtists>(response.Content.ReadAsStream()) ?? throw new Exception("500");
            artists.AddRange(_artists.artists.Where(x => x is not null).Select(x => new Artist(x, ids.FirstOrDefault(y => y.Item1 == x.id)?.Item2)));
        }
    }
}
