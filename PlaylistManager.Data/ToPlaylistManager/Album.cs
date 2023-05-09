namespace PlaylistManager.Data.ToPlaylistManager
{
    public class Album
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public long? Timestamp { get; set; }

        public Album(FromSpotify.Album? album, long? timestamp = null)
        {
            Url = album?.external_urls?.spotify ?? "";
            Id = album?.id ?? "" ;
            Name = album?.name ?? "";
            Image = /*album?.images?.FirstOrDefault(x => x.width == 300)?.url ??*/ album?.images?.FirstOrDefault()?.url;
            Timestamp = timestamp;
        }
    }
}
