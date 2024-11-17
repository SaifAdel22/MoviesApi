using Microsoft.EntityFrameworkCore;

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

    }
}
