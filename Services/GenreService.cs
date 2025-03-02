using BookReviewApi.Models;
using BookReviewApi.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

// Genre interface is reposnsible for the Crud operations perfomed on genre 
public class GenreService : IGenreService // Inherits from interface which provides methods 
{
    private readonly ApplicationContext _context; // Aplication context to interact with the genre table

    public GenreService(ApplicationContext context) // Contructor to inject depenendency and initialise database context
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllGenresAsync() // Retrieves list of genres 
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<Genre> GetGenreByIdAsync(int id) // Finds genre entity with the same id in database 
    {
        return await _context.Genres.FindAsync(id); // If not found null is returned 
    }

    public async Task AddGenreAsync(Genre genre) // Adds a genre to the database
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync(); // Any changes are saved in the database
    }

    public async Task UpdateGenreAsync(int id, Genre genre) // Updates genre entity
    {
       _context.Entry(genre).State = EntityState.Modified; // Genre entity is modified
        await _context.SaveChangesAsync(); // Any changes are saved in the database 
    }
    public async Task DeleteGenreAsync(int id) // Deletes a genre entity with matching id
    {
        var genre = await _context.Genres.FindAsync(id); // Finds genre with the same id in database 
        if (genre != null) // If genre with id is found
        {
            _context.Genres.Remove(genre); // Removed and changes are saved 
            await _context.SaveChangesAsync(); 
        }
        
    }
}
