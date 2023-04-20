using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlaylistManager.Services
{
    public interface IUtils
    {
        ActionResult ErrorManager(Exception ex);
        string StatusCode(HttpResponseMessage response);
        public string GetReturnUrl(IHeaderDictionary headers);
    }
}