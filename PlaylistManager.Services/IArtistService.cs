using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface IArtistService
    {
        List<Artist> GetArtists(string token, List<Tuple<string, long>> ids);
    }
}