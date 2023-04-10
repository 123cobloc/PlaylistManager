namespace PlaylistManager.Data.ToPlaylistManager
{
    public class Token
    {
        public string AccessToken { get; set; }
        public long Expires { get; set; }
        public string RefreshToken { get; set; }

        public Token(FromSpotify.Token token)
        {
            AccessToken = token.access_token;
            RefreshToken = token.refresh_token;
            Expires = DateTimeOffset.UtcNow.AddSeconds(token.expires_in - 10).ToUnixTimeMilliseconds();
        }
    }
}
