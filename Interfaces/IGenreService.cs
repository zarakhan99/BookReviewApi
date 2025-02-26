using BookReviewApi.Models;
public interface IGenreService
{
    Task<IEnumerable<Genre>> GetAllGenresAsync();
    Task<Genre> GetGenreByIdAsync(int id);
    Task <Genre> AddGenreAsync(Genre genre);
    Task UpdateGenreAsync(int id, Genre genre);
    Task DeleteGenreAsync(int id);
}