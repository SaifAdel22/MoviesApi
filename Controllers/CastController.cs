using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastController(ICastServ castServ, IMoviesServ moviesServ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddAsync(CastDTO castDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Cast cast = new Cast
            {
                Name = castDTO.Name,
                Age = castDTO.Age,
                Description = castDTO.Description,
            };
            var result = await castServ.Add(cast);
            if (result != null)
                return Ok(cast);

            return BadRequest();

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var list = await castServ.GetAll();
            return Ok(list);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(CastDTO castDTO, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var exist = await castServ.GetById(id);
            if (exist == null)
            {
                return NotFound($"No Actor was found with ID: {id}");
            }

            exist.Name = castDTO.Name;
            exist.Age = castDTO.Age;
            exist.Description = castDTO.Description;

            castServ.Update(exist);
            return Ok(exist);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exist = await castServ.GetById(id); 
            if(exist == null)   
                return NotFound($"No Actor was found with ID: {id}");

            await castServ.Remove(exist); 
            return Ok(exist);
        }
        [HttpPost("AssignCastToMovie/{castId}/{movieId}")]
        public async Task<IActionResult> AssignCastToMovie(int castId, int movieId)
        {
            // Check if the cast member exists
            var cast = await castServ.GetById(castId);
            if (cast == null)
                return NotFound($"No Actor was found with ID: {castId}");

            // Check if the movie exists
            var movie = await moviesServ.GetById(movieId);
            if (movie == null)
                return NotFound($"No Movie was found with ID: {movieId}");

            // Associate the cast member with the movie
            if (cast.MovieCasts == null) // Ensure the collection is initialized
                cast.MovieCasts = new List<MovieCast>();

            cast.MovieCasts.Add(new MovieCast
            {
                CastId = castId,
                MovieId = movieId
            });

            // Save the changes
             castServ.Update(cast);
            return Ok($"Actor with ID: {castId} has been assigned to Movie with ID: {movieId}");
        }
        [HttpGet("GetCastByMovie/{movieId}")]
        public async Task<IActionResult> GetCastByMovie(int movieId)
        {
            var castList = await castServ.GetCastByMovie(movieId);
            if (castList == null || !castList.Any())
                return NotFound($"No cast members found for Movie with ID: {movieId}");

            return Ok(castList);
        }

        [HttpGet("GetMoviesByCast/{castId}")]
        public async Task<IActionResult> GetMoviesByCast(int castId)
        {
            var movieList = await moviesServ.GetMoviesByCast(castId);
            if (movieList == null || !movieList.Any())
                return NotFound($"No movies found for Actor with ID: {castId}");

            return Ok(movieList);
        }


    }
}
