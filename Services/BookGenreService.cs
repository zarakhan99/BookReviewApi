using BookReviewApi.Models;
using BookReviewApi.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

//BookGenre interface is reposnsible for the Crud operations  perfomed on bookgenre 
public class BookGenreService : IBookGenreService // Inherits from interface which provides methods 
{
    private readonly ApplicationContext _context; // Aplication context to interacdt with the bookgenre table

    public BookGenreService(ApplicationContext context) //Contructor to inject depenendency and initialise database context
    {
        _context = context;
    }

    public async Task<IEnumerable<BookGenre>> GetBookGenresAsync()
    {
        return await _context.BookGenres.ToListAsync(); // Retrieves list of bookgenres 
    }

    public async Task <BookGenre> GetBookGenreByIdAsync(int id) // Finds bookgenre with the same id in database 
    {
        return await _context.BookGenres.FindAsync(id); // If not found null is returned 
    }

    public async Task UpdateBookGenreAsync(int id, BookGenre bookGenre) // Updates bookgenre entity
    {
       _context.Entry(bookGenre).State = EntityState.Modified; // Bookgenre entity is modified
       await _context.SaveChangesAsync(); // Any changes are saved in the database 
    }

    public async Task AddBookGenreAsync(BookGenre bookGenre) // Adds a book genre to the database 
    {
        _context.BookGenres.Add(bookGenre); 
        await _context.SaveChangesAsync(); // Any changes are saved in the database 
    }

    public async Task DeleteBookGenreAsync(int id) // Deletes a book genre entity with matching id 
    {
        var bGenre = await _context.BookGenres.FindAsync(id); // Finds bookgenre with the same id in database 
        if (bGenre != null) // If bookgenre with id found 
        {
            _context.BookGenres.Remove(bGenre); // Removed and changes are saved 
            await _context.SaveChangesAsync();
        }
         
    }
}