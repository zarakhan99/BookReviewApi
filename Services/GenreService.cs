using BookReviewApi.Models;
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
        var genre = await _context.Genres.FindAsync(id);
         if (genre == null)
        {
            throw new KeyNotFoundException($"Genre with ID {id} was not found.");
        }
        return genre;

    }

    public async Task <Genre> AddGenreAsync(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task UpdateGenreAsync(int id, Genre genre)
    {
       var existingGenre = await _context.Genres.FindAsync(id);
       if(existingGenre == null)
       {
         throw new KeyNotFoundException($"Genre with ID {id} was not found.");
       }

        if (id != genre.GenreId)
    {
        throw new ArgumentException("ID does not match Genre ID.");
    }
    // Update fields
    existingGenre.GenreName = genre.GenreName;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        throw new Exception("Error updating Genre, possibly due to a concurrency conflict.");
    }
    }

    public async Task DeleteGenreAsync(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre != null)
        {
            throw new KeyNotFoundException($"Genre with ID {id} was not found.");
        }
        _context.Genres.Remove(genre);
        
        await _context.SaveChangesAsync();
    }
}
