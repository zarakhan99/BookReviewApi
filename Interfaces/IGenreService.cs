using BookReviewApi.Models;
public interface IGenreService // interface with http methods to be used in GenereService
{
    Task<IEnumerable<Genre>> GetAllGenresAsync();
    Task<Genre> GetGenreByIdAsync(int id);
    Task AddGenreAsync(Genre genre);
    Task UpdateGenreAsync(int id, Genre genre);
    Task DeleteGenreAsync(int id);
}