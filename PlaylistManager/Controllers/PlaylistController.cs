using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistManager.Data.ToPlaylistManager;
using PlaylistManager.Services;

namespace PlaylistManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        [HttpGet("all")]
        public ActionResult<Playlist> GetMyPlaylists()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                return Ok(_playlistService.GetMyPlaylists(token));
            }
            catch (Exception ex)
            {
                return ex.Message == "Unauthorized" ? Unauthorized() : BadRequest(ex.Message);
            }
        }

        [HttpGet("queue")]
        public ActionResult<Playlist> GetQueue()
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                return Ok(_playlistService.LoadTracks(token, _playlistService.GetQueue(token) ?? throw new Exception("Playlist not found.")));
            }
            catch (Exception ex)
            {
                return ex.Message == "Unauthorized" ? Unauthorized() : BadRequest(ex.Message);
            }
        }

        [HttpGet("{playlistId}")]
        public ActionResult<Playlist> GetPlaylist(string playlistId)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                Playlist playlist = _playlistService.GetPlaylist(token, playlistId) ?? throw new Exception("Playlist not found.");
                return Ok(_playlistService.LoadTracks(token, playlist));
            }
            catch (Exception ex)
            {
                return ex.Message == "Unauthorized" ? Unauthorized() : BadRequest(ex.Message);
            }
        }
    }
}
