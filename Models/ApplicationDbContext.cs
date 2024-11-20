using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<MovieCast> MovieCasts { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Movie)
                .WithMany(m => m.Feedbacks)
                .HasForeignKey(f => f.MovieId);

            // MovieCast relationships (Many-to-Many between Movie and Cast)
            modelBuilder.Entity<MovieCast>()
                .HasKey(mc => new { mc.MovieId, mc.CastId });  // Composite key

            modelBuilder.Entity<MovieCast>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.MovieCasts)
                .HasForeignKey(mc => mc.MovieId);

            modelBuilder.Entity<MovieCast>()
                .HasOne(mc => mc.Cast)
                .WithMany(c => c.MovieCasts)
                .HasForeignKey(mc => mc.CastId);

            modelBuilder.Entity<WatchList>()
       .HasKey(wl => new { wl.MovieId, wl.UserId });

            modelBuilder.Entity<WatchList>()
                .HasOne(wl => wl.Movie)
                .WithMany(m => m.WatchLists)
                .HasForeignKey(wl => wl.MovieId);

            modelBuilder.Entity<WatchList>()
                .HasOne(wl => wl.User)
                .WithMany(u => u.WatchLists)
                .HasForeignKey(wl => wl.UserId);
        }
    }
}
