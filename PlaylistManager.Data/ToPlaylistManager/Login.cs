using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data.ToPlaylistManager
{
    public class Login
    {
        public string Url { get; set; }
        public string State { get ; set; }

        public Login(string url, string state)
        {
            Url = url;
            State = state;
        }
    }
}
