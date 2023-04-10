using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface IPlaylistService
    {
        List<Playlist> GetMyPlaylists(string token);
        Playlist? GetPlaylist(string token, string playlistId);
        Playlist? GetQueue(string token);
        bool AddTrack(string token, string playlistId, string trackUri);
        bool RemoveTrack(string token, string playlistId, string trackUri);
        Playlist LoadTracks(string token, Playlist playlist);

    }
}