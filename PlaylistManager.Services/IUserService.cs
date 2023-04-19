using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface IUserService
    {
        Login GenerateLoginUrl(string codeVerifier);
        Token GetToken(string authorizationCode, string codeVerifier);
        Token RefreshToken(string refreshToken);
        User GetMe(string token);
    }
}