using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface IPlaylistService
    {
        void AddTrack(string token, string playlistId, string trackId);
        PlaylistService.ContainsTrack CheckTrack(string token, string playlistId, string trackId);
        List<Playlist> GetMyPlaylists(string token);
        Playlist GetPlaylist(string token, string playlistId);
        Playlist LoadTracks(string token, Playlist playlist);
        void RemoveTrack(string token, string playlistId, string trackId);
    }
}