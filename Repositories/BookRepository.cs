using Microsoft.EntityFrameworkCore;
using BookReviewApi.Context;
using BookReviewApi.Models;

// Book repository to act as a layer between the databse and controllers 

namespace BookReviewApi.Repositories 
{
    public class BookRepository
    {
        private readonly ApplicationContext _bookContext; // Aplication context to interact with the book table

        public BookRepository(ApplicationContext bookContext) // Contructor to inject depenendency and initialise database context
        {
            _bookContext = bookContext;
        }

        public async Task<IEnumerable<Book>> GetAllAsync() => await _bookContext.Books.ToListAsync(); // Retrieves list of all books 

        public async Task<Book> GetByIdAsync(int id) => await _bookContext.Books.FindAsync(id); // Finds book entity with the same id in database 

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(int genreId) // Retrieves books based on genre id 
        {
            return await _bookContext.Books
            .Where(b => b.BookGenres.Any(g => g.GenreId == genreId)) // Checks which books are associated with the specific genre id through bookgenre
            .ToListAsync(); // Runs query and lists asynchronously
        }

        public async Task AddAsync(Book book) // Adds a book to the database
        {
            _bookContext.Books.Add(book); 
            await _bookContext.SaveChangesAsync(); // Any changes are saved in the database 
        }

        public async Task UpdateAsync(Book book) // Updates book entity
        {
            _bookContext.Entry(book).State = EntityState.Modified; // Book entity is modified
            await _bookContext.SaveChangesAsync(); // Any changes are saved in the database
        }

        public async Task DeleteAsync(int id) // Finds book with the same id in database
        {
            var book = await _bookContext.Books.FindAsync(id);
            if (book != null) // If book with id found
            {
                _bookContext.Books.Remove(book); // Removed and changes are saved 
                await _bookContext.SaveChangesAsync(); 
            }
        }
    }
}
