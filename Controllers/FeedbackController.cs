using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using MoviesApi.DTO;
using MoviesApi.Models;
using MoviesApi.Services;

[Route("api/[controller]")]
[ApiController]
public class FeedbackController : ControllerBase
{
    private readonly IMoviesServ moviesServ;
    private readonly IFeedbackServ feedbackServ;
    private readonly UserManager<ApplicationUser> userManager;

    public FeedbackController(IMoviesServ moviesServ, IFeedbackServ feedbackServ, UserManager<ApplicationUser> userManager)
    {
        this.moviesServ = moviesServ;
        this.feedbackServ = feedbackServ;
        this.userManager = userManager;
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> SubmitFeedback(FeedbackDTO feedback)
    {
        if (ModelState.IsValid)
        {
            var movie = await moviesServ.GetById(feedback.MovieId);
            if (movie == null)
            {
                return NotFound("Movie not found.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not authenticated.");

            // Parse the user ID as a Guid
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("Invalid user ID format.");
            }

            var user = await userManager.FindByIdAsync(parsedUserId.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var existingFeedback = await feedbackServ.GetById(feedback, parsedUserId);

            if (existingFeedback != null)
            {
                // Update the review (can be updated multiple times)
                existingFeedback.Review = feedback.Review ?? existingFeedback.Review;

                // Always save the last rating submitted
                existingFeedback.Rating = feedback.Rating ?? existingFeedback.Rating;

                existingFeedback.CreatedAt = DateTime.UtcNow;
                existingFeedback.UserId = parsedUserId;

                feedbackServ.Update(existingFeedback);
            }
            else
            {
                // Create new feedback if no existing feedback
                Feedback newFeedback = new Feedback
                {
                    UserId = parsedUserId,
                    MovieId = feedback.MovieId,
                    CreatedAt = DateTime.UtcNow,
                    Review = feedback.Review,
                    Rating = feedback.Rating
                };

                await feedbackServ.Add(newFeedback); // Use AddAsync for async operation
            }

            return Ok("Feedback submitted successfully.");
        }

        return BadRequest(ModelState);
    }
}
