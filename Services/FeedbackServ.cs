using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApi.Services
{
    public class FeedbackServ : IFeedbackServ
    {
        private readonly ApplicationDbContext context;

        public FeedbackServ(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Feedback> Add(Feedback feedback)
        {
            await context.AddAsync(feedback);
            await context.SaveChangesAsync(); // Use async SaveChangesAsync
            return feedback;
        }

        public async Task<List<FeedbackDTO>> GetAllRev(int id)
        {
            return await context.Feedbacks
                .Where(x => x.MovieId == id)
                .Select(x => new FeedbackDTO
                {
                    Review = x.Review,
                    Rating = x.Rating ?? 0
                })
                .ToListAsync();
        }

        public async Task<Feedback> GetById(FeedbackDTO feedback, Guid userId)
        {
            var existingFeedback = await context.Feedbacks
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == feedback.MovieId);
            return existingFeedback;
        }



        public async Task<decimal> GetRate(int id)
        {
            var averageRating = await context.Feedbacks
                .Where(f => f.MovieId == id && f.Rating != null)
                .AverageAsync(f => (decimal?)f.Rating) ?? 0;
            return averageRating;
        }

        public Feedback Update(Feedback feedback)
        {
            context.Update(feedback);
            context.SaveChanges(); // synchronous SaveChanges is okay here, or use async SaveChangesAsync if preferred
            return feedback;
        }
    }
}
