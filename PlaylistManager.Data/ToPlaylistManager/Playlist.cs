using Microsoft.IdentityModel.Tokens;
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
            Image = playlist.images.FirstOrDefault()?.url;
            Name = playlist.name;
            Description = playlist.description.IsNullOrEmpty() ? null : playlist.description;
            IsMine = userId is null ? null : playlist.owner.id == userId;
            IsCollaborative = playlist.collaborative;
            TracksNumber = playlist.tracks.total;
            Timestamp = timestamp;
        }
    }
}
