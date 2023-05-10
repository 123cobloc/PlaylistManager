namespace PlaylistManager.Data.ToPlaylistManager
{
    public class Track
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get => Album.Image; }
        public List<Artist> Artists { get; set; }
        public Album Album { get; set; }
        public long? Timestamp { get; set; }
        public bool? IsFromQueue { get; set; } = null;

        public Track(FromSpotify.Track track, long? timestamp = null)
        {
            Url = track.external_urls.spotify ?? "";
            Id = track.id ?? "";
            Name = track.name ?? "";
            Artists = track.artists.Select(a => new Artist(a)).ToList();
            Album = new Album(track.album);
            Timestamp = timestamp;
        }
    }
}
