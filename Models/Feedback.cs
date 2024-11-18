namespace MoviesApi.Models
{
    public class Feedback
    {
        public int Id {get; set;}
        [Range(1,10)]
        public decimal? Rating { get; set;}
        [MaxLength(255)]
        public string? Review { get; set;}
        public DateTime? CreatedAt { get; set;}
        public Guid UserId {  get; set;}
        public ApplicationUser? User { get; set;}
        public int MovieId {  get; set;}
        public Movie? Movie { get; set;}
    }
}
