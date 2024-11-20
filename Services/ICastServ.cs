namespace MoviesApi.Services
{
    public interface ICastServ
    {
        Task<Cast> GetById(int id);
        Task<Cast> Add(Cast cast);
        Task<Cast> Remove(Cast cast);
        Task<List<Cast>> GetAll();
        Cast Update(Cast cast);
        Task<List<CastDTO>> GetCastByMovie(int movieId);

    }
}
