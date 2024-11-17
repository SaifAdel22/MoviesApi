using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTO;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IGenresServ genresServ;
        private readonly IMoviesServ moviesServ;
        private readonly IMapper mapper;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;

        public MoviesController(IGenresServ genresServ, IMoviesServ moviesServ, IMapper mapper)
        {
            this.genresServ = genresServ;
            this.moviesServ = moviesServ;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await moviesServ.GetAll();

            var data = mapper.Map<IEnumerable<MovieDetailsDTO>>(movies);


            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await moviesServ.GetById(id);

            if (movie == null)
                return NotFound();

            var dto = mapper.Map<MovieDetailsDTO>(movie);
            return Ok(dto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await moviesServ.GetAll(genreId);

            var data = mapper.Map<IEnumerable<MovieDetailsDTO>>(movies);


            return Ok(data);
        }
        [Authorize]

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDTO dto)
        {
            if(!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            {
                return BadRequest("Only Jpg and Png");
            }
           if(dto.Poster.Length > _maxAllowedPosterSize)
            {
                return BadRequest("Too Big");
            }
            var ISValidGenre = await genresServ.IsvalidGenre(dto.GenreId);
            if (ISValidGenre == false)
            {
                return BadRequest("Genre NOt Exist");
            }
                using var dataStream = new MemoryStream();

            await dto.Poster.CopyToAsync(dataStream);

            var movie = mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();
            moviesServ.Add(movie);

            return Ok(movie);
        }
        [Authorize]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDTO dto)
        {
            var movie = await moviesServ.GetById(id);

            if (movie == null)
                return NotFound($"No movie was found with ID {id}");

            var isValidGenre = await genresServ.IsvalidGenre(dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genere ID!");

            if (dto.Poster != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();

                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;
            movie.StoreLine = dto.Storeline;
            movie.Rate = dto.Rate;

            moviesServ.Update(movie);

            return Ok(movie);
        }
        [Authorize]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await moviesServ.GetById(id);

            if (movie == null)
                return NotFound($"No movie was found with ID {id}");

            moviesServ.Delete(movie);

            return Ok(movie);
        }

    }
}
