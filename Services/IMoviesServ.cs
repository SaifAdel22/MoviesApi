﻿namespace MoviesApi.Services
{
    public interface IMoviesServ
    {
        Task<IEnumerable<Movie>> GetAll(byte genreId = 0);
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);
        Task<List<MovieCastDTO>> GetMoviesByCast(int castId);
    }
}
