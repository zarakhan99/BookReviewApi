using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BookReviewApi.Models
{
public class ApplicationContext : IdentityDbContext<IdentityUser> //providing built in tables to mange user, roles and authentication
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) // using dbcontext to build cutom tables 
        {
        }
        // Creating tables for each model 
        public DbSet<Member> Members { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Review> Reviews { get; set; }


	}
}
