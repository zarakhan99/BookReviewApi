using Microsoft.EntityFrameworkCore; // Required for ToListAsync(), FindAsync(), and EntityState
using BookReviewApi.Models;

namespace BookReviewApi.Repositories
{
    public class ReviewRepository
    {
        private readonly ApplicationContext _reviewContext;

        public ReviewRepository(ApplicationContext reviewContext)
        {
            _reviewContext = reviewContext;
        }

        public async Task<IEnumerable<Review>> GetAllAsync() => await _reviewContext.Reviews.ToListAsync();

        public async Task<Review> GetByIdAsync(int id) => await _reviewContext.Reviews.FindAsync(id);

        public async Task<List<Review>> GetReviewsForBookAsync(int bookId)
        {
            return await _reviewContext.Reviews
                .Where(r => r.BookId == bookId)
                .ToListAsync();
        }

        public async Task AddAsync(Review review)
        {
            _reviewContext.Reviews.Add(review);
            await _reviewContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            _reviewContext.Entry(review).State = EntityState.Modified;
            await _reviewContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
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
