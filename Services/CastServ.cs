
namespace MoviesApi.Services
{
    public class CastServ(ApplicationDbContext context) : ICastServ
    {
        public async Task<Cast> Add(Cast cast)
        {
            await context.AddAsync(cast);
            await context.SaveChangesAsync();
            return cast;
        }

        public async Task<List<Cast>> GetAll()
        {
            return await context.Casts.OrderBy(x=>x.Name).ToListAsync();
        }

        public async Task<Cast> GetById(int id)
        {
            return await context.Casts.SingleOrDefaultAsync(x => x.Id == id );
        }

        public async Task<Cast> Remove(Cast cast)
        {
            context.Casts.Remove(cast);
            await context.SaveChangesAsync();
            return cast;
        }

        public Cast Update(Cast cast)
        {
            context.Casts.Update(cast);
            context.SaveChanges();
            return cast;
        }
        public async Task<List<CastDTO>> GetCastByMovie(int movieId)
        {
            var castList = await context.MovieCasts
                .Where(mc => mc.MovieId == movieId)
                .Include(mc => mc.Cast)  // Eager load the Cast entity
                .Select(mc => mc.Cast)// Select the Cast entity directly
                .Select(x=> new CastDTO
                {
                    Age=x.Age,
                    Name=x.Name,
                    Description=x.Description,
                })
                .ToListAsync();

            return castList;
        }

    }
}
