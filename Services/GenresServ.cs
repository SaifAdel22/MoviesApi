

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Services
{
    public class GenresServ :IGenresServ
    {
        private readonly ApplicationDbContext context;

        public GenresServ(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
        }
        public async Task<Genre> Add(Genre genre)
        {
            await context.AddAsync(genre);
            context.SaveChanges();

            return genre;
        }

        public async  Task <Genre> Delete(Genre genre)
        {

            context.Remove(genre);
            context.SaveChanges();
            return genre ;
        }

        public async Task <List<Genre>> GetAll() { return await context.Genres.OrderBy(s=>s.Name).ToListAsync(); }

        public async Task <Genre> GetById(int id)
        {
            Genre genre = await context.Genres.SingleOrDefaultAsync(x=>x.Id==id);
           
            return genre;
        }

        public  Task<bool> IsvalidGenre(byte id)
        {
            return context.Genres.AnyAsync(x => x.Id == id); ;
        }

        public Genre Update(Genre genre)
        {
            context.Update(genre);
            context.SaveChanges();

            return genre;
        }

    }
}
