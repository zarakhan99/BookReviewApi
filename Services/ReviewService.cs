using BookReviewApi.Models;
using BookReviewApi.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookReviewApi.Services  //BookGenre interface is reposnsible for the Crud operations  perfomed on bookgenre 
{ 
    public class ReviewService : IReviewService //Inherits from interface which provides methods 
    {
        private readonly ApplicationContext _context; // Application context to interact with the review table
        
        public ReviewService(ApplicationContext context) // Contructor to inject depenendency and initialise database context
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Review>> GetAllReviewsAsync() // Retrieves list of reviews 
        {
            return await _context.Reviews.ToListAsync();
        }
        
        public async Task<Review> GetReviewByIdAsync(int id) // Finds review entity with the same id in database 
        {
            return await _context.Reviews.FindAsync(id); // If not found null is returned
        }
        
        public async Task<IEnumerable<Review>> GetReviewsForBookAsync(int bookId) // Gets reviews for specific book using book id
        {
            return await _context.Reviews
            .Where(r => r.BookId == bookId) // Checks if reviews are associated with a specific book id
            .ToListAsync(); // Runs query and lists asynchronously
        }
        
        public async Task AddReviewAsync(Review review) // Adds a review to the database
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync(); // Any changes are saved in the database
        }
        
        public async Task UpdateReviewAsync(int id, Review review) // Updates review entity
        {
            _context.Entry(review).State = EntityState.Modified; // Review entity is modified
            await _context.SaveChangesAsync(); // Any changes are saved in the database 
        }
        
        public async Task DeleteReviewAsync(int id) // Deletes a review entity with matching id
        {
            var review = await _context.Reviews.FindAsync(id); // Finds review with the same id in database 
            if (review != null) // If review with id is found
            {
                _context.Reviews.Remove(review); // Removed and changes are saved
                await _context.SaveChangesAsync();
            }
        }
    }
}

