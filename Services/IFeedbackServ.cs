namespace MoviesApi.Services
{
    public interface IFeedbackServ
    {
        //Task<Feedback> GetFeedbackById(int id);
        //Task<Feedback> Exist(FeedbackDTO feedback,string userid);
        Task<Feedback> GetById(FeedbackDTO feedback,Guid userid);
         Feedback Update(Feedback feedback);
        Task<Feedback> Add(Feedback feedback);
        Task <List<FeedbackDTO>> GetAllRev(int id);
        Task<decimal> GetRate(int id);
    }
}
