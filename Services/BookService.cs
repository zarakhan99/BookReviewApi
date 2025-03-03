using BookReviewApi.Models;
using BookReviewApi.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookReviewApi.Services // Book interface is reposnsible for the Crud operations perfomed on book entities
{
    public class BookService : IBookService //Inherits from interface which provides methods 
    {
        private readonly ApplicationContext _context; // Aplication context to interact with the book table
        
        public BookService(ApplicationContext context) // Contructor to inject depenendency and initialise database context
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()  // Retrieves list of all books 
        {
            return await _context.Books.ToListAsync();
        }

        public async Task <Book> GetBookByIdAsync(int id) // Finds book entity with the same id in database 
        {
            return await _context.Books.FindAsync(id); // If not found, null is returned 
        }

        public async Task <IEnumerable<Book>> GetBookByGenreAsync(int genreId) // Retrieves books based on genre id 
        {
            var books = await _context.Books
            .Where(b => b.BookGenres.Any(g => g.GenreId == genreId)) // Checks which books are associated with the specific genre id through bookgenre
            .ToListAsync(); // Runs query and lists asynchronously
            
            return books; // Returns books that match genre id
        }

        public async Task AddBookAsync(Book book) // Adds a book to the database
        {
            _context.Books.Add(book); 
            await _context.SaveChangesAsync(); // Any changes are saved in the database 
        }
        
        public async Task UpdateBookAsync(int id, Book book) // Updates bookg entity
        {
            _context.Entry(book).State = EntityState.Modified; // Book entity is modified
            await _context.SaveChangesAsync(); // Any changes are saved in the database 
        }
        
        public async Task DeleteBookAsync(int id) // Deletes a book entity with matching id 
        {
            var book = await _context.Books.FindAsync(id); // Finds book with the same id in database
            if (book != null) // If book with id found
            {
                _context.Books.Remove(book); // Removed and changes are saved 
                await _context.SaveChangesAsync();
            }
        }
    }

}

