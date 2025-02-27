using BookReviewApi.Models;

public interface IReviewService // interface with service methods to be used in ReviewService
{
    Task<IEnumerable<Review>> GetAllReviewsAsync();
    Task<Review> GetReviewByIdAsync(int id);
    Task<IEnumerable<Review>> GetReviewsForBookAsync(int bookId);
    Task UpdateReviewAsync(int id, Review review);
    Task AddReviewAsync(Review review);
    Task DeleteReviewAsync(int id);
}