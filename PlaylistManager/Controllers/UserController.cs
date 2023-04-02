using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistManager.Data.FromSpotify;
using PlaylistManager.Services;

namespace PlaylistManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(UserService userService)
        { 
            _userService = userService;
        }

        [HttpGet("loginUrl")]
        public ActionResult<string> GetLoginUrl(string codeVerifier)
        {
            return codeVerifier.Length == 128 ? Ok(_userService.GenerateLoginUrl(codeVerifier)) : BadRequest("Invalid codeVerifier");
        }

        [HttpGet("login")]
        public ActionResult<Token> GetToken(string authorizationCode, string codeVerifier)
        {
            try
            {
                return Ok(_userService.GetToken(authorizationCode, codeVerifier));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("me")]
        public ActionResult<User> GetMe()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                return Ok(_userService.GetMe(token));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
