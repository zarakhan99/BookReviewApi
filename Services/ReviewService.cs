using BookReviewApi.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class ReviewService : IReviewService
{
    private readonly ApplicationContext _context;

    public ReviewService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetAllReviewsAsync()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<Review> GetReviewByIdAsync(int id)
    {
        return await _context.Reviews.FindAsync(id);
    }

    public async Task<IEnumerable<Review>> GetReviewsForBookAsync(int bookId)
    {
        return await _context.Reviews
        .Where(r => r.BookId == bookId) //checks if reviews are associated with a specific book id
        .ToListAsync();

    }

    public async Task AddReviewAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateReviewAsync(int id, Review review)
    {
        _context.Entry(review).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
       
    }
}
