namespace MoviesApi.DTO
{
    public class GenreDTO
    {
        [MaxLength(100)]
        public string Name {  get; set; }
    }
}
