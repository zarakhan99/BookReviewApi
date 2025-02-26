using BookReviewApi.Models;

public interface IReviewService
{
    Task<IEnumerable<Review>> GetAllReviewsAsync();
    Task<Review> GetReviewByIdAsync(int id);
    Task<IEnumerable<Review>> GetReviewsForBookAsync(int bookId);
    Task UpdateReviewAsync(int id, Review review);
    Task<Review> AddReviewAsync(Review review);
    Task DeleteReviewAsync(int id);
}