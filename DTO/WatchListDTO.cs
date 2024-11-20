namespace MoviesApi.DTO
{
    public class WatchListDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public double Rate { get; set; }
        public int Year { get; set; }
    }
}
