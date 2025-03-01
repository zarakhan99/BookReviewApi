using BookReviewApi.Models;
using BookReviewApi.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class GenreService : IGenreService
{
    private readonly ApplicationContext _context;

    public GenreService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllGenresAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<Genre> GetGenreByIdAsync(int id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task AddGenreAsync(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateGenreAsync(int id, Genre genre)
    {
       _context.Entry(genre).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteGenreAsync(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre != null)
        {
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
        
    }
}
