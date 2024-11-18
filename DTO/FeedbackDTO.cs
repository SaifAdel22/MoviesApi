namespace MoviesApi.DTO
{
    public class FeedbackDTO
    {
        [Range(1, 10)]
        public decimal? Rating { get; set; }
        [MaxLength(255)]
        public string? Review { get; set; }
        //DateTime? CreatedAt { get; set; }
        //public int UserId { get; set; }
        //public ApplicationUser? User { get; set; }
        public int MovieId { get; set; }
        //public Movie? Movie { get; set; }
    }

}
