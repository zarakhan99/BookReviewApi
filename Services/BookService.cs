using BookReviewApi.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class BookService : IBookService
{
    private readonly ApplicationContext _context;

    public BookService(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _context.Books.ToListAsync();
    }

    public async Task <Book> GetBookByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
 
    }

    public async Task <IEnumerable<Book>> GetBookByGenreAsync(int genreId)
    {
       var books = await _context.Books
        .Where(b => b.BookGenres.Any(g => g.GenreId == genreId)) //checks which books are associated with the specific genre id
        .ToListAsync();

        return books;
    }

    public async Task <Book> AddBookAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task UpdateBookAsync(int id, Book book)
    {
        _context.Entry(book).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
        
    }
}