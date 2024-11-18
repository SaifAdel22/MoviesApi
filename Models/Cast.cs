namespace MoviesApi.Models
{
    public class Cast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string? Description { get; set; }
        public List<MovieCast> MovieCasts { get; set; } // Navigation property


    }
}
