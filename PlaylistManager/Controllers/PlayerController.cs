using Microsoft.AspNetCore.Mvc;
using PlaylistManager.Data.ToPlaylistManager;
using PlaylistManager.Services;

namespace PlaylistManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IUtils _utils;
        private readonly IPlayerService _playerService;
        public PlayerController(Utils utils, PlayerService playerService)
        {
            _utils = utils;
            _playerService = playerService;
        }

        [HttpGet("current")]
        public ActionResult<Track> GetCurrentlyPlaying()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                return _playerService.GetCurrentTrack(token);
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }
    }
}
