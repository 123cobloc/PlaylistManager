using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface IAlbumService
    {
        List<Album> GetAlbums(string token, List<Tuple<string, long>> ids);
    }
}