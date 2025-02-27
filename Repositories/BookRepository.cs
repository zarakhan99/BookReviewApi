using Microsoft.EntityFrameworkCore;
using BookReviewApi.Models;

namespace BookReviewApi.Repositories
{
    public class BookRepository
    {
        private readonly ApplicationContext _bookContext;

        public BookRepository(ApplicationContext bookContext)
        {
            _bookContext = bookContext;
        }

        public async Task<IEnumerable<Book>> GetAllAsync() => await _bookContext.Books.ToListAsync();

        public async Task<Book> GetByIdAsync(int id) => await _bookContext.Books.FindAsync(id);

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId)
        {
            return await _bookContext.Books
            .Where(b => b.BookGenres.Any(g => g.GenreId == genreId)) 
            .ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            _bookContext.Books.Add(book);
            await _bookContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _bookContext.Entry(book).State = EntityState.Modified;
            await _bookContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _bookContext.Books.FindAsync(id);
            if (book != null)
            {
                _bookContext.Books.Remove(book);
                await _bookContext.SaveChangesAsync();
            }
        }
    }
}
