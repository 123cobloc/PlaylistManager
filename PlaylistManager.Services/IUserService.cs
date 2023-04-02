using PlaylistManager.Data.ToPlaylistManager;

namespace PlaylistManager.Services
{
    public interface IUserService
    {
        string GenerateLoginUrl(string codeVerifier);
        Token GetToken(string authorizationCode, string codeVerifier);
        User GetMe(string token);
    }
}