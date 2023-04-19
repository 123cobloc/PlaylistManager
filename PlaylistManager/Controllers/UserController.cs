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
        public ActionResult<string> GetLoginUrl(string codeVerifier)
        {
            return codeVerifier.Length == 128 ? Ok(_userService.GenerateLoginUrl(codeVerifier)) : BadRequest("Invalid codeVerifier");
        }

        [HttpGet("token")]
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
