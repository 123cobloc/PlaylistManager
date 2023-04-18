using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Data.FromSpotify
{

    public class Player
    {
        public Device device { get; set; }
        public bool shuffle_state { get; set; }
        public string repeat_state { get; set; }
        public long timestamp { get; set; }
        public Context? context { get; set; }
        public int progress_ms { get; set; }
        public Track? item { get; set; }
        public string currently_playing_type { get; set; }
        public Actions actions { get; set; }
        public bool is_playing { get; set; }
    }

    public class Device
    {
        public string id { get; set; }
        public bool is_active { get; set; }
        public bool is_private_session { get; set; }
        public bool is_restricted { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int volume_percent { get; set; }
    }

    public class Context
    {
        public External_Urls external_urls { get; set; }
        public string href { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Actions
    {
        public Disallows disallows { get; set; }
    }

    public class Disallows
    {
        public bool resuming { get; set; }
    }

}
