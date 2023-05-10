namespace PlaylistManager.Data.ToPlaylistManager
{
    public class Playlist
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string? Image { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? IsMine { get; set; }
        public bool IsCollaborative { get; set; }
        public int TracksNumber { get; set; }
        public List<Track> Tracks { get; set; } = new();
        public long? Timestamp { get; set; }

        public Playlist(FromSpotify.Playlist playlist, string? userId = null, long? timestamp = null)
        {
            Url = playlist.external_urls.spotify;
            Id = playlist.id;
            Image = playlist.images.FirstOrDefault(x => x.width == 300)?.url ?? playlist.images.FirstOrDefault()?.url;
            Name = playlist.name;
            Description = string.IsNullOrEmpty(playlist.description) ? null : playlist.description;
            IsMine = userId is null ? null : playlist.owner.id == userId;
            IsCollaborative = playlist.collaborative;
            TracksNumber = playlist.tracks.total;
            Timestamp = timestamp;
        }
    }
}
