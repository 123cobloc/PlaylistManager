using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface IUserService
    {
        Login GenerateLoginUrl(string codeVerifier, string redirectUri);
        Token GetToken(string authorizationCode, string codeVerifier, string redirectUri);
        Token RefreshToken(string refreshToken);
        User GetMe(string token);
    }
}