using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data.ToPlaylistManager
{
    public class Album
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }

        public Album(FromSpotify.Album? album)
        {
            Url = album?.external_urls?.spotify ?? "";
            Id = album?.id ?? "" ;
            Name = album?.name ?? "";
            Image = album?.images?.FirstOrDefault()?.url ?? "";
        }
    }
}
