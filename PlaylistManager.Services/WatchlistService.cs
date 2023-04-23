using PlaylistManager.Data;
using PlaylistManager.Data.ToPlaylistManager;
using System.Text.Json;

namespace PlaylistManager.Services
{
    public class WatchlistService : IWatchlistService
    {

        private readonly IUtils _utils;
        private readonly IUserService _userService;
        private readonly ITrackService _trackService;
        private readonly IAlbumService _albumService;
        private readonly IArtistService _artistService;
        private readonly IPlaylistService _playlistService;
        private readonly PlaylistManagerCosmos _cosmos;
        public WatchlistService(Utils utils, UserService userService, TrackService trackService, AlbumService albumService, ArtistService artistService, PlaylistService playlistService, PlaylistManagerCosmos cosmos)
        {
            _utils = utils;
            _userService = userService;
            _trackService = trackService;
            _albumService = albumService;
            _artistService = artistService;
            _playlistService = playlistService;
            _cosmos = cosmos;
        }

        public void AddToWatchlist(string token, string playlistId, ItemType itemType)
        {
            string userId = _userService.GetMe(token).Id;
            if (_cosmos.Watchlist.Where(x => x.UserId == userId && x.ItemId == playlistId && x.ItemType == itemType).FirstOrDefault() is not null) throw new Exception("409");
            _cosmos.Add(new Watchlist(userId, playlistId, itemType));
            if (_cosmos.SaveChanges() == 0) throw new Exception("Generic error.");
        }

        public void RemoveFromWatchlist(string token, string playlistId, ItemType itemType)
        {
            string userId = _userService.GetMe(token).Id;
            Watchlist item = _cosmos.Watchlist.FirstOrDefault(x => x.UserId == userId && x.ItemId == playlistId && x.ItemType == itemType) ?? throw new Exception("404");
            _cosmos.Watchlist.Remove(item);
            if (_cosmos.SaveChanges() == 0) throw new Exception("Generic error.");
        }

        public List<object> SearchFor(string token, ItemType itemType, string query)
        {
            string _itemType = itemType switch
            {
                ItemType.Album => "album",
                ItemType.Artist => "artist",
                ItemType.Playlist => "playlist",
                ItemType.Track => "track",
                _ => throw new Exception("400"),
            };

            HttpClient httpClient = _utils.HttpClient(token);
            HttpResponseMessage response = httpClient.GetAsync($"https://api.spotify.com/v1/search?query={query.Replace(' ', '+')}&type={_itemType}&limit={50}").Result;
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));

            switch (itemType)
            {
                case ItemType.Album:
                    Data.FromSpotify.AlbumsSearch? albumsSearch = JsonSerializer.Deserialize<Data.FromSpotify.AlbumsSearch>(response.Content.ReadAsStream());
                    return albumsSearch is not null ? albumsSearch.albums.items.Select(x => new Album(x)).Cast<object>().ToList() : throw new Exception("500");
                case ItemType.Artist:
                    Data.FromSpotify.ArtistsSearch? artistsSearch = JsonSerializer.Deserialize<Data.FromSpotify.ArtistsSearch>(response.Content.ReadAsStream());
                    return artistsSearch is not null ? artistsSearch.artists.items.Select(x => new Artist(x)).Cast<object>().ToList() : throw new Exception("500");
                case ItemType.Playlist:
                    Data.FromSpotify.PlaylistsSearch? playlistsSearch = JsonSerializer.Deserialize<Data.FromSpotify.PlaylistsSearch>(response.Content.ReadAsStream());
                    return playlistsSearch is not null ? playlistsSearch.playlists.items.Select(x => new Playlist(x)).Cast<object>().ToList() : throw new Exception("500");
                case ItemType.Track:
                    Data.FromSpotify.TracksSearch? tracksSearch = JsonSerializer.Deserialize<Data.FromSpotify.TracksSearch>(response.Content.ReadAsStream());
                    return tracksSearch is not null ? tracksSearch.tracks.items.Select(x => new Track(x)).Cast<object>().ToList() : throw new Exception("500");
                default:
                    throw new Exception("500");
            }
        }

        public List<object> GetWatchlist(string token, ItemType itemType)
        {
            string userId = _userService.GetMe(token).Id;
            List<Watchlist> watchlist = _cosmos.Watchlist.Where(x => x.UserId == userId && x.ItemType == itemType).ToList();
            List<object> result = new();
            switch (itemType)
            {
                case ItemType.Album:
                    result.AddRange(_albumService.GetAlbums(token, watchlist.Select(x => new Tuple<string, long>(x.ItemId, x.Timestamp)).ToList()));
                    break;
                case ItemType.Artist:
                    result.AddRange(_artistService.GetArtists(token, watchlist.Select(x => new Tuple<string, long>(x.ItemId, x.Timestamp)).ToList()));
                    break;
                case ItemType.Playlist:
                    result.AddRange(_playlistService.GetPlaylists(token, watchlist.Select(x => new Tuple<string, long>(x.ItemId, x.Timestamp)).ToList()));
                    break;
                case ItemType.Track:
                    result.AddRange(_trackService.GetTracks(token, watchlist.Select(x => new Tuple<string, long>(x.ItemId, x.Timestamp)).ToList()));
                    break;
                default:
                    throw new Exception("400");
            };
            return result;
        }
    }
}
