using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface ITrackService
    {
        Track GetTrack(string token, string trackId);
        List<Track> GetTracks(string token, List<Tuple<string, long>> ids);
    }
}