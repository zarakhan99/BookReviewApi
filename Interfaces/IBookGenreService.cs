using BookReviewApi.Models;

public interface IBookGenreService
{
    Task <IEnumerable<BookGenre>> GetBookGenresAsync();
    Task <BookGenre> GetBookGenreByIdAsync(int id);
    Task UpdateBookGenreAsync(int id, BookGenre bookGenre);
    Task <BookGenre> AddBookGenreAsync(BookGenre bookGenre);
    Task DeleteBookGenreAsync(int id);
}