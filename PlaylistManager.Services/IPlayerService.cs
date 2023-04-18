using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface IPlayerService
    {
        Track GetCurrentTrack(string token);
    }
}