namespace MoviesApi.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public int Year { get; set; }
        public double Rate {  get; set; }

        [MaxLength(2550)]

        public string StoreLine {  get; set; }
        public byte[] Poster { get; set; }

        public Genre? Genre { get; set; }
        public byte GenreId { get; set; }
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public List<MovieCast> MovieCasts { get; set; } // Navigation property
        public List<WatchList>?WatchLists { get; set; } // Navigation property

    }
}
