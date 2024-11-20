
namespace MoviesApi.Services
{
    public class WatchListServ(ApplicationDbContext context) : IWatchListServ
    {
        public async Task<WatchList> Add(WatchList watchList)
        {
            await context.AddAsync(watchList);
            await context.SaveChangesAsync();
            return watchList;
        }

        public WatchList Delete(WatchList watchList)
        {
            context.Remove(watchList);
            context.SaveChanges();
            return watchList;
        }

        public async Task <WatchList> Exist(int movieId , Guid userId)
        {
            var exist = await context.WatchLists.SingleOrDefaultAsync(x=> x.UserId == userId && x.MovieId == movieId);
            return exist;
        }


        public async Task<List<WatchListDTO>> GetByAllMoviesByUserId(Guid userId)
        {
            var list = await context.WatchLists
                .Where(wl => wl.UserId == userId)
                .Select(wl => new WatchListDTO
                {
                    MovieId = wl.Movie.Id,
                    Title = wl.Movie.Title,
                    Genre = wl.Movie.Genre.Name,
                    Rate = wl.Movie.Rate,
                    Year = wl.Movie.Year
                })
                .ToListAsync();

            return list;
        }
    }
}
