using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistManager.Services
{
    public class Utils : IUtils
    {
        public ActionResult ErrorManager(Exception ex)
        {
            return int.TryParse(ex.Message, out int statusCode) ? new StatusCodeResult(statusCode) : new BadRequestObjectResult(ex.Message);
            //return ex.Message.ToLower() switch
            //{
            //    "unauthorized" => new UnauthorizedResult(),
            //    "not found" => new NotFoundResult(),
            //    "bad request" => new BadRequestResult(),
            //    "forbidden" => new ForbidResult(),
            //    "too many requests" => new StatusCodeResult(429),

            //    _ => new BadRequestObjectResult(ex.Message),
            //};
        }

        public string StatusCode(HttpResponseMessage response)
        {
            return ((int)response.StatusCode).ToString();
        }
    }
}
