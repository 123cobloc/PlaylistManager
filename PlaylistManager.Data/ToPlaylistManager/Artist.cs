using PlaylistManager.Data.FromSpotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data.ToPlaylistManager
{
    public class Artist
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }

        public Artist(FromSpotify.Artist? artist)
        {
            Url = artist?.external_urls?.spotify ?? "";
            Id = artist?.id ?? "";
            Name = artist?.name ?? "";
        }
    }
}
