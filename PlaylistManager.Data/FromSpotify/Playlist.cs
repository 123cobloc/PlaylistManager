using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data.FromSpotify
{

    public class PlaylistPaginator
    {
        public string href { get; set; }
        public List<Playlist> items { get; set; }
        public int limit { get; set; }
        public string? next { get; set; }
        public int offset { get; set; }
        public string? previous { get; set; }
        public int total { get; set; }
    }

    public class Playlist
    {
        public bool collaborative { get; set; }
        public string description { get; set; }
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public string name { get; set; }
        public Owner owner { get; set; }
        public object primary_color { get; set; }
        public bool _public { get; set; }
        public string snapshot_id { get; set; }
        public PlaylistTracks tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Owner
    {
        public string display_name { get; set; }
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class PlaylistTracks
    {
        public string href { get; set; }
        public int total { get; set; }
    }


    public class PlaylistsSearch
    {
        public Playlists playlists { get; set; }
    }

    public class Playlists
    {
        public string href { get; set; }
        public List<Playlist> items { get; set; }
        public int limit { get; set; }
        public string? next { get; set; }
        public int offset { get; set; }
        public string? previous { get; set; }
        public int total { get; set; }
    }
}
