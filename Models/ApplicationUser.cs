using System.Security.Principal;

namespace MoviesApi.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public List<WatchList>? WatchLists { get; set; } 

    }
}
