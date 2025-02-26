using BookReviewApi.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class BookGenreService : IBookGenreService
{
    private readonly ApplicationContext _context;

    public BookGenreService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookGenre>> GetBookGenresAsync()
    {
        return await _context.BookGenres.ToListAsync();
    }

    public async Task <BookGenre> GetBookGenreByIdAsync(int id)
    {
        var bookGenre = await _context.BookGenres.FindAsync(id);
        if (bookGenre == null)
        {
            throw new KeyNotFoundException($"BookGenre with ID {id} was not found.");
        }
        return bookGenre;
    }

    public async Task UpdateBookGenreAsync(int id, BookGenre bookGenre)
    {
       var exBookGenre = await _context.BookGenres.FindAsync(id);
       if(exBookGenre == null)
       {
        throw new KeyNotFoundException($"BookGenre with ID {id} was not found.");
       }
       exBookGenre.BookId = bookGenre.BookId;
       exBookGenre.GenreId = bookGenre.GenreId;

       await _context.SaveChangesAsync(); 
    }

    public async Task <BookGenre> AddBookGenreAsync(BookGenre bookGenre)
    {
        _context.BookGenres.Add(bookGenre);
        await _context.SaveChangesAsync();
        
        return bookGenre;
    }

    public async Task DeleteBookGenreAsync(int id)
    {
        var bGenre = await _context.BookGenres.FindAsync(id);
        if (bGenre != null)
        {
           throw new KeyNotFoundException($"BookGenre with ID {id} was not found.");
        }
         _context.BookGenres.Remove(bGenre);

        await _context.SaveChangesAsync();
    }
}