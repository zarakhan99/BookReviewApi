using BookReviewApi.Models;

public interface IBookService
{
    Task <IEnumerable<Book>> GetAllBooksAsync();
    Task <Book> GetBookByIdAsync(int id);
    Task <IEnumerable<Book>> GetBookByGenreAsync(int genreId);
    Task UpdateBookAsync(int id, Book book);
    Task<Book> AddBookAsync(Book book);
    Task DeleteBookAsync(int id);
}