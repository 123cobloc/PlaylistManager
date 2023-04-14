using Microsoft.AspNetCore.Mvc;

namespace PlaylistManager.Services
{
    public interface IUtils
    {
        ActionResult ErrorManager(Exception ex);
        string StatusCode(HttpResponseMessage response);
    }
}