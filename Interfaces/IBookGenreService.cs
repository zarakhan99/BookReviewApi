using BookReviewApi.Models;

public interface IBookGenreService // interface with http methods to be used in BookGenereService
{
    Task <IEnumerable<BookGenre>> GetBookGenresAsync();
    Task <BookGenre> GetBookGenreByIdAsync(int id);
    Task UpdateBookGenreAsync(int id, BookGenre bookGenre);
    Task AddBookGenreAsync(BookGenre bookGenre);
    Task DeleteBookGenreAsync(int id);
}