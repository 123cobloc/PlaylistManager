using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface IWatchlistService
    {
        void AddToWatchlist(string token, string playlistId, ItemType itemType);
        List<object> GetWatchlist(string token, ItemType itemType);
        void RemoveFromWatchlist(string token, string playlistId, ItemType itemType);
        List<object> SearchFor(string token, ItemType itemType, string query);
    }
}