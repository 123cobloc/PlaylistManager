using Microsoft.AspNetCore.Mvc;
using PlaylistManager.Data.ToPlaylistManager;
using PlaylistManager.Services;

namespace PlaylistManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IUtils _utils;
        private readonly IPlaylistService _playlistService;

        public PlaylistController(Utils utils, PlaylistService playlistService)
        {
            _utils = utils;
            _playlistService = playlistService;
        }

        [HttpGet("all")]
        public ActionResult<Playlist> GetMyPlaylists()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token)) throw new Exception("401");
                List<Playlist> playlists = _playlistService.GetMyPlaylists(token);
                if (playlists.Any(x => x.Name == "Queue")) playlists.Remove(playlists.FirstOrDefault(x => x.Name == "Queue")!);
                return Ok(playlists);
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }

        [HttpGet("{playlistId}")]
        public ActionResult<Playlist> GetPlaylist(string playlistId)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token)) throw new Exception("401");
                return Ok(_playlistService.GetPlaylist(token, playlistId));
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }

        [HttpPost("{playlistId}/add/{trackId}")]
        public ActionResult AddTrack(string playlistId, string trackId)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token)) throw new Exception("401");
                _playlistService.AddTrack(token, playlistId, trackId);
                return Ok();
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }

        [HttpDelete("{playlistId}/remove/{trackId}")]
        public ActionResult RemoveTrack(string playlistId, string trackId)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token)) throw new Exception("401");
                _playlistService.RemoveTrack(token, playlistId, trackId);
                return Ok();
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }

        [HttpGet("{playlistId}/contains/{trackId}")]
        public ActionResult CheckTrack(string playlistId, string trackId)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token)) throw new Exception("401");
                return Ok(_playlistService.CheckTrack(token, playlistId, trackId));
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }

        [HttpPost("CreateQueue")]
        public ActionResult CreateQueue()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token)) throw new Exception("401");
                _playlistService.CreateQueue(token);
                return Ok();
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }
    }
}
