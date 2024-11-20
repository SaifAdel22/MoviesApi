namespace MoviesApi.Services
{
    public interface IWatchListServ
    {
       Task<WatchList> Add(WatchList watchList);
        WatchList Delete(WatchList watchList);
        Task<WatchList> Exist(int movieId,Guid userId);
        Task<List<WatchListDTO>> GetByAllMoviesByUserId(Guid userid);
    }
}
