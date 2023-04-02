using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data.ToPlaylistManager
{
    public class User
    {
        public string Username { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
        public string? Image { get; set; }
        public bool IsPremium { get; set; }

        public User(FromSpotify.User user)
        {
            Username = user.display_name;
            Url = user.external_urls.spotify;
            Id = user.id;
            Image = user.images.FirstOrDefault()?.url;
            IsPremium = user.product == "premium";
        }
    }
}
