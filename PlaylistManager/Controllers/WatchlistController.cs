using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlaylistManager.Data.FromSpotify;
using PlaylistManager.Data.ToPlaylistManager;
using PlaylistManager.Services;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PlaylistManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly IUtils _utils;
        private readonly IWatchlistService _watchlistService;

        public WatchlistController(Utils utils, WatchlistService watchlistService)
        {
            _utils = utils;
            _watchlistService = watchlistService;
        }

        [HttpPost("add/{itemType}/{itemId}")]
        public ActionResult AddToWatchlist(ItemType itemType, string itemId)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                _watchlistService.AddToWatchlist(token, itemId, itemType);
                return Ok();
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }

        [HttpDelete("remove/{itemType}/{itemId}")]
        public ActionResult RemoveFromWatchlist(ItemType itemType, string itemId)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                _watchlistService.RemoveFromWatchlist(token, itemId, itemType);
                return Ok();
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }

        [HttpGet("search/{itemType}")]
        public ActionResult<object> SearchFor(ItemType itemType, string query)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                if (token.IsNullOrEmpty()) throw new Exception("401");
                return Ok(_watchlistService.SearchFor(token, itemType, query));
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }

        [HttpGet("{itemType}")]
        public ActionResult<List<object>> GetWatchlist(ItemType itemType)
        {
            try
            {
                string token = HttpContext.Request.Headers["Authorization"].ToString();
                if (token.IsNullOrEmpty()) throw new Exception("401");
                return Ok(_watchlistService.GetWatchlist(token, itemType));
            }
            catch (Exception ex)
            {
                return _utils.ErrorManager(ex);
            }
        }
    }
}
