using BookReviewApi.Models;
using BookReviewApi.Context;
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
        return await _context.BookGenres.FindAsync(id);
    }

    public async Task UpdateBookGenreAsync(int id, BookGenre bookGenre)
    {
       _context.Entry(bookGenre).State = EntityState.Modified;
       await _context.SaveChangesAsync();
    }

    public async Task AddBookGenreAsync(BookGenre bookGenre)
    {
        _context.BookGenres.Add(bookGenre);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookGenreAsync(int id)
    {
        var bGenre = await _context.BookGenres.FindAsync(id);
        if (bGenre != null)
        {
            _context.BookGenres.Remove(bGenre);
            await _context.SaveChangesAsync();
        }
         
    }
}