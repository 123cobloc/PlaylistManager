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
    }
}
