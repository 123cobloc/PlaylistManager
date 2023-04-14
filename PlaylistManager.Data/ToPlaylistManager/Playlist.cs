using PlaylistManager.Data.FromSpotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data.ToPlaylistManager
{
    public class Playlist
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string? Image { get; set; }
        public string Name { get; set; }
        public bool IsMine { get; set; }
        public bool IsCollaborative { get; set; }
        public int TracksNumber { get; set; }
        public List<Track> Tracks { get; set; } = new();

        public Playlist(FromSpotify.Playlist playlist, string userId)
        {
            Url = playlist.external_urls.spotify;
            Id = playlist.id;
            Image = playlist.images.First().url;
            Name = playlist.name;
            IsMine = playlist.owner.id == userId;
            IsCollaborative = playlist.collaborative;
            TracksNumber = playlist.tracks.total;
        }
    }
}
