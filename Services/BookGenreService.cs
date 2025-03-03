using BookReviewApi.Models;
using BookReviewApi.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookReviewApi.Services
{
    // BookGenreService is responsible for performing CRUD operations on BookGenre
    public class BookGenreService : IBookGenreService
    {
        private readonly ApplicationContext _context; // Application context to interact with the BookGenre table

        // Constructor to inject dependency and initialize the database context
        public BookGenreService(ApplicationContext context)
        {
            _context = context;
        }

        // Retrieves the list of book genres from the database
        public async Task<IEnumerable<BookGenre>> GetBookGenresAsync()
        {
            return await _context.BookGenres.ToListAsync();
        }

        // Finds a book genre by ID from the database
        public async Task<BookGenre> GetBookGenreByIdAsync(int id)
        {
            return await _context.BookGenres.FindAsync(id); // If not found, null is returned
        }

        // Updates the BookGenre entity in the database
        public async Task UpdateBookGenreAsync(int id, BookGenre bookGenre)
        {
            _context.Entry(bookGenre).State = EntityState.Modified; // BookGenre entity is modified
            await _context.SaveChangesAsync(); // Changes are saved to the database
        }

        // Adds a new BookGenre to the database
        public async Task AddBookGenreAsync(BookGenre bookGenre)
        {
            _context.BookGenres.Add(bookGenre);
            await _context.SaveChangesAsync(); // Changes are saved to the database
        }

        // Deletes a BookGenre entity by ID from the database
        public async Task DeleteBookGenreAsync(int id)
        {
            var bGenre = await _context.BookGenres.FindAsync(id); // Finds the BookGenre with the given ID
            if (bGenre != null) // If found, remove it
            {
                _context.BookGenres.Remove(bGenre); // BookGenre is removed from the context
                await _context.SaveChangesAsync(); // Changes are saved to the database
            }
        }
    }
}
