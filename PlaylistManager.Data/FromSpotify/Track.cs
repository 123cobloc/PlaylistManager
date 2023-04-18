using PlaylistManager.Data.ToPlaylistManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data.FromSpotify
{
    public class TracksPaginator
    {
        public string href { get; set; }
        public List<Item> items { get; set; }
        public int limit { get; set; }
        public string? next { get; set; }
        public int offset { get; set; }
        public string? previous { get; set; }
        public int total { get; set; }
    }

    public class Item
    {
        public DateTime added_at { get; set; }
        public Added_By added_by { get; set; }
        public bool is_local { get; set; }
        public object primary_color { get; set; }
        public Track track { get; set; }
        public Video_Thumbnail video_thumbnail { get; set; }
    }

    public class Added_By
    {
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Track
    {
        public Album album { get; set; }
        public List<Artist> artists { get; set; }
        public List<string> available_markets { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool? episode { get; set; }
        public bool _explicit { get; set; }
        public External_Ids external_ids { get; set; }
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public bool is_local { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string preview_url { get; set; }
        public bool? track { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class External_Ids
    {
        public string isrc { get; set; }
    }

    public class Video_Thumbnail
    {
        public object url { get; set; }
    }

    public class TracksSearch
    {
        public Tracks tracks { get; set; }
    }

    public class Tracks
    {
        public string href { get; set; }
        public List<Track> items { get; set; }
        public int limit { get; set; }
        public string? next { get; set; }
        public int offset { get; set; }
        public string? previous { get; set; }
        public int total { get; set; }
    }

    public class GetTracks
    {
        public List<Track> tracks { get; set; }
    }
}
