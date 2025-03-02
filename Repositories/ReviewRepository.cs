using Microsoft.EntityFrameworkCore;
using BookReviewApi.Context; 
using BookReviewApi.Models;

// Review repository to act as a layer between the databse and controllers
namespace BookReviewApi.Repositories
{
    public class ReviewRepository
    {
        private readonly ApplicationContext _reviewContext; // Application context to interact with the review table

        public ReviewRepository(ApplicationContext reviewContext) // Contructor to inject depenendency and initialise database context
        {
            _reviewContext = reviewContext;
        }

        public async Task<IEnumerable<Review>> GetAllAsync() => await _reviewContext.Reviews.ToListAsync(); // Retrieves list of reviews

        public async Task<Review> GetByIdAsync(int id) => await _reviewContext.Reviews.FindAsync(id); // Finds review entity with the same id in database 

        public async Task<List<Review>> GetReviewsForBookAsync(int bookId) // Gets reviews for specific book using book id
        {
            return await _reviewContext.Reviews
                .Where(r => r.BookId == bookId)
                .ToListAsync();
        }

        public async Task AddAsync(Review review) // Adds a review to the database
        {
            _reviewContext.Reviews.Add(review);
            await _reviewContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Review review) // Updates review entity
        {
            _reviewContext.Entry(review).State = EntityState.Modified;
            await _reviewContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) // Deletes a review entity with matching id
        {
            var review = await _reviewContext.Reviews.FindAsync(id);
            if (review != null)
            {
                _reviewContext.Reviews.Remove(review);
                await _reviewContext.SaveChangesAsync();
            }
        }
    }
}
