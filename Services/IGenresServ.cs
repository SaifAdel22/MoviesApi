namespace MoviesApi.Services
{
    public interface IGenresServ
    {
       Task <List<Genre>> GetAll();
        Task <Genre> GetById(int id);
        Task<bool> IsvalidGenre(byte id);
        Task<Genre> Add(Genre genre);

        Task <Genre> Delete(Genre genre);

        Genre Update(Genre genre);

    }
}
