using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MoviesApi.DTO;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresServ genresServ;
        private readonly IMoviesServ moviesServ;

        public GenresController(IGenresServ genresServ,IMoviesServ moviesServ)
        {
            this.genresServ = genresServ;
            this.moviesServ = moviesServ;
        }

        [HttpGet]
        public async Task <IActionResult> GetAllAsync()
        {
            var genres = await genresServ.GetAll(); // Await here
            return Ok(genres);
        }

        [Authorize]

        [HttpPost]
        public async Task <IActionResult> CreateAstnc(GenreDTO createGenreDTO)
        {
            Genre genre = new Genre{ Name = createGenreDTO.Name };
            await genresServ.Add(genre);
            return Ok(genre);
        }
        [Authorize]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, GenreDTO dto)
        {
            var genre = await genresServ.GetById(id);
         

            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            genre.Name = dto.Name;

            genresServ.Update(genre);

            return Ok(genre);
        }
        [Authorize]

        [HttpDelete("{id}")]
        public async Task <IActionResult> DeleteAsync(int  id)
        {
            var genre = await genresServ.GetById(id);


            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");

            genresServ.Delete(genre);
            return Ok(genre);
        }
    }
}
