using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlaylistManager.Services
{
    public class Utils : IUtils
    {
        public ActionResult ErrorManager(Exception ex)
        {
            return int.TryParse(ex.Message, out int statusCode) ? new StatusCodeResult(statusCode) : new BadRequestObjectResult(ex.Message);
        }

        public string StatusCode(HttpResponseMessage response)
        {
            return ((int)response.StatusCode).ToString();
        }

        public string GetReturnUrl(IHeaderDictionary headers)
        {
            string returnUrl = headers["Origin"].ToString();
            return $"{(string.IsNullOrEmpty(returnUrl) ? "http://test" : returnUrl)}/callback";
        }

        public HttpClient HttpClient(string? token = null)
        {
            HttpClient httpClient = new();
            if (token is not null) httpClient.DefaultRequestHeaders.Add("Authorization", token);
            return httpClient;
        }
    }
}
