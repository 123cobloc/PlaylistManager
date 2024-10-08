﻿using PlaylistManager.Data.ToPlaylistManager;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PlaylistManager.Services
{
    public class UserService : IUserService
    {
        private readonly IUtils _utils;
        public UserService(Utils utils)
        { 
            _utils = utils;
        }

        public Login GenerateLoginUrl(string codeVerifier, string redirectUri)
        {
            string baseUrl = "https://accounts.spotify.com/authorize";
            string responseType = "code";
            string clientId = "8ebd57c9f29644fda8054ad400676c43";
            string scope = "user-read-private user-read-email playlist-read-private playlist-modify-public playlist-modify-private user-read-playback-state user-modify-playback-state";
            string state = GenerateRandomString(16);
            string codeChallengeMethod = "S256";
            string codeChallenge = GenerateCodeChallenge(codeVerifier);

            return new Login(baseUrl + $"?response_type={responseType}&client_id={clientId}&scope={scope}&redirect_uri={redirectUri}&state={state}&code_challenge_method={codeChallengeMethod}&code_challenge={codeChallenge}", state);
        }
        
        public Token GetToken(string authorizationCode, string codeVerifier, string redirectUri)
        {
            Dictionary<string, string> values = new()
            {
                { "grant_type", "authorization_code" },
                { "code", authorizationCode },
                { "redirect_uri", redirectUri },
                { "client_id", "8ebd57c9f29644fda8054ad400676c43" },
                { "code_verifier", codeVerifier }
            };

            FormUrlEncodedContent body = new(values);

            HttpClient httpClient = _utils.HttpClient();
            HttpResponseMessage response = httpClient.PostAsync("https://accounts.spotify.com/api/token", body).Result;

            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));

            Data.FromSpotify.Token? token = JsonSerializer.Deserialize<Data.FromSpotify.Token>(response.Content.ReadAsStream());

            return token is not null ? new Token(token) : throw new Exception("Generic error.");
        }

        public Token RefreshToken(string refreshToken)
        {
            Dictionary<string, string> values = new()
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", "8ebd57c9f29644fda8054ad400676c43" }
            };

            FormUrlEncodedContent body = new(values);

            HttpClient httpClient = _utils.HttpClient();
            HttpResponseMessage response = httpClient.PostAsync("https://accounts.spotify.com/api/token", body).Result;

            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));

            Data.FromSpotify.Token? token = JsonSerializer.Deserialize<Data.FromSpotify.Token>(response.Content.ReadAsStream());

            return token is not null ? new Token(token) : throw new Exception("Generic error.");
        }

        public User GetMe(string token)
        {
            HttpClient httpClient = _utils.HttpClient(token);
            HttpResponseMessage response = httpClient.GetAsync("https://api.spotify.com/v1/me").Result;
            if (!response.IsSuccessStatusCode) throw new Exception(_utils.StatusCode(response));
            Data.FromSpotify.User? user = JsonSerializer.Deserialize<Data.FromSpotify.User>(response.Content.ReadAsStream());
            return user is not null ? new User(user) : throw new Exception("Generic error.");
        }

        private string GenerateRandomString(int lenght)
        {
            return Guid.NewGuid().ToString("n").Substring(0, lenght);
        }

        private string GenerateCodeChallenge(string codeVerifier)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            string base64 = Convert.ToBase64String(hash)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
            return base64;
        }
    }
}
