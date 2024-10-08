﻿namespace PlaylistManager.Data.FromSpotify
{
    public class User
    {
        public string country { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public Explicit_Content explicit_content { get; set; }
        public External_Urls external_urls { get; set; }
        public Followers followers { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public Image[] images { get; set; }
        public string product { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Explicit_Content
    {
        public bool filter_enabled { get; set; }
        public bool filter_locked { get; set; }
    }

    public class External_Urls
    {
        public string spotify { get; set; }
    }

    public class Followers
    {
        public object href { get; set; }
        public int total { get; set; }
    }

    public class Image
    {
        public int? height { get; set; }
        public string url { get; set; }
        public int? width { get; set; }
    }

}
