using Microsoft.AspNetCore.Mvc;
using PlaylistManager.Data.ToPlaylistManager;
using PlaylistManager.Services;

namespace PlaylistManager.Controllers
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
        public ActionResult<Login> GetLoginUrl(string codeVerifier)
        {
            string url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/callback";
            return codeVerifier.Length == 128 ? Ok(_userService.GenerateLoginUrl(codeVerifier, url)) : BadRequest("Invalid codeVerifier");
        }

        [HttpGet("test")]
        public ActionResult<object> Test()
        {
            string? returnUrl = HttpContext.Request.Headers["X-Forwarded-For"] /*? HttpContext.Request.Form["returnurl"].ToString() : "http://test" */;
            return Ok(new { test = $"{returnUrl}/callback" });
        }

        [HttpGet("token")]
        public ActionResult<Token> GetToken(string authorizationCode, string codeVerifier)
        {
            try
            {
                string url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/callback";
                return Ok(_userService.GetToken(authorizationCode, codeVerifier, url));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("refreshToken")]
        public ActionResult<Token> RefreshToken(string refreshToken)
        {
            try
            {
                return Ok(_userService.RefreshToken(refreshToken));
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
                if (string.IsNullOrEmpty(token)) throw new Exception("401");
                return Ok(_userService.GetMe(token));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
