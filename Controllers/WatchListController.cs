using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController(IMoviesServ moviesServ, IWatchListServ watchListServ, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private Guid? GetUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
                return null;

            if (!Guid.TryParse(userIdString, out var userId))
                return null;

            return userId;
        }

        [HttpPost("AddMovieToYourWatchList/{movieid}")]
        public async Task<IActionResult> Add([FromRoute] int movieid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = await moviesServ.GetById(movieid);

            if (movie == null)
                return NotFound("No Movie With This Id");

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("Invalid or missing user ID.");

            var watchList = new WatchList
            {
                UserId = userId.Value,
                MovieId = movieid,
            };
            await watchListServ.Add(watchList);

            return Ok("Movie Added To WatchList Successfully");
        }

        [HttpDelete("RemoveMovieFromYourWatchList/{movieid}")]
        public async Task<IActionResult> Delete([FromRoute] int movieid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = await moviesServ.GetById(movieid);

            if (movie == null)
                return NotFound("No Movie With This Id");

            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("Invalid or missing user ID.");

            var del = await watchListServ.Exist(movieid, userId.Value);

            if (del == null)
                return NotFound("Movie Doesn't Exist in Your List");

            watchListServ.Delete(del);

            return Ok("Movie Removed From Your WatchList Successfully");
        }

        [HttpGet("GetAllMoviesInYourWatchList")]
        public async Task<IActionResult> GetAllMovies()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized("Invalid or missing user ID.");

            var movies = await watchListServ.GetByAllMoviesByUserId(userId.Value);
            if (movies == null || !movies.Any())
                return NotFound("No movies found in your watchlist.");

            return Ok(movies);
        }
    }
}
