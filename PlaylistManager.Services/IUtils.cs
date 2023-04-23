using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlaylistManager.Services
{
    public interface IUtils
    {
        ActionResult ErrorManager(Exception ex);
        string StatusCode(HttpResponseMessage response);
        string GetReturnUrl(IHeaderDictionary headers);
        HttpClient HttpClient(string? token = null);
    }
}