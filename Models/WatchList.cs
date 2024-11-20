namespace MoviesApi.Models
{
    public class WatchList
    {
        public int MovieId {  get; set; }
        public Movie? Movie { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
