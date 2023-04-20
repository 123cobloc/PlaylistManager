using Microsoft.AspNetCore.Mvc;
using PlaylistManager.Data.ToPlaylistManager;
using PlaylistManager.Services;

namespace PlaylistManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUtils _utils;
        private readonly IUserService _userService;
        public UserController(Utils utils, UserService userService)
        { 
            _utils = utils;
            _userService = userService;
        }

        [HttpGet("loginUrl")]
        public ActionResult<Login> GetLoginUrl(string codeVerifier)
        {
            return codeVerifier.Length == 128 ? Ok(_userService.GenerateLoginUrl(codeVerifier, _utils.GetReturnUrl(HttpContext.Request.Headers))) : BadRequest("Invalid codeVerifier");
        }

        [HttpGet("token")]
        public ActionResult<Token> GetToken(string authorizationCode, string codeVerifier)
        {
            try
            {
                return Ok(_userService.GetToken(authorizationCode, codeVerifier, _utils.GetReturnUrl(HttpContext.Request.Headers)));
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
