using Microsoft.EntityFrameworkCore;

namespace BookReviewApi.Models
{
public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        // Creating tables for each model
        public DbSet<Member> Members { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
	}
}
