namespace PlaylistManager.Data.FromSpotify
{
    public class ArtistsSearch
    {
        public Artists artists { get; set; }
    }

    public class Artists
    {
        public string href { get; set; }
        public List<Artist> items { get; set; }
        public int limit { get; set; }
        public string? next { get; set; }
        public int offset { get; set; }
        public string? previous { get; set; }
        public int total { get; set; }
    }

    public class Artist
    {
        public External_Urls external_urls { get; set; }
        public Followers? followers { get; set; }
        public List<string>? genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image>? images { get; set; }
        public string name { get; set; }
        public int? popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }


    public class GetArtists
    {
        public List<Artist> artists { get; set; }
    }

}
