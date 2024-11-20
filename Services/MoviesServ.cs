using Microsoft.EntityFrameworkCore;
using MoviesApi.DTO;

namespace MoviesApi.Services
{
    public class MoviesServ : IMoviesServ
    {
        private readonly ApplicationDbContext context;

        public MoviesServ(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
        }
        public async Task<Movie> Add(Movie movie)
        {
            await context.AddAsync(movie);
            context.SaveChanges();

            return movie;
        }

        public Movie Delete(Movie movie)
        {
            context.Remove(movie);
            context.SaveChanges();

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(byte genreId = 0)
        {
            return await context.Movies
                .Where(m => m.GenreId == genreId || genreId == 0)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genre)
                .ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
        }

        public Movie Update(Movie movie)
        {
            context.Update(movie);
            context.SaveChanges();

            return movie;
        }
        public async Task<List<MovieCastDTO>> GetMoviesByCast(int castId)
        {
            var movieList = await context.MovieCasts
                .Where(mc => mc.CastId == castId)
                .Include(mc => mc.Movie)  // Eager load the Movie entity
                .Select(mc => mc.Movie)   // Select the Movie entity directly
                .Select(movie => new MovieCastDTO
                {
                    Title = movie.Title,
                    Year = movie.Year,
                    Rate = movie.Rate,
                    Storeline = movie.StoreLine,
                    // Do not include 'Poster' or 'MovieCasts' in the projection
                })
                .ToListAsync();

            return movieList;
        }


    }
}
