namespace MoviesApi.DTO
{
    public class FeedbackRate_ReviewDTO
    {
        [Range(1, 10)]
        public decimal? Rating { get; set; }
        [MaxLength(255)]
        public string? Review { get; set; }
    }
}
