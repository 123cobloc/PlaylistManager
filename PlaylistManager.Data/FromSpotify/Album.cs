namespace PlaylistManager.Data.FromSpotify
{

    public class AlbumsSearch
    {
        public Albums albums { get; set; }
    }

    public class Albums
    {
        public string href { get; set; }
        public List<Album> items { get; set; }
        public int limit { get; set; }
        public string? next { get; set; }
        public int offset { get; set; }
        public string? previous { get; set; }
        public int total { get; set; }
    }

    public class Album
    {
        public string album_group { get; set; }
        public string album_type { get; set; }
        public List<Artist> artists { get; set; }
        public List<string> available_markets { get; set; }
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public bool? is_playable { get; set; }
        public string name { get; set; }
        public string release_date { get; set; }
        public string release_date_precision { get; set; }
        public int total_tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class GetAlbums
    {
        public List<Album> albums { get; set; }
    }
}
