using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BookReviewApi.Models;

//Controller handles book endpoints for creating, retriving, updating, and deleting books - uses BookService to perform operations
namespace BookReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService; // Book service interface 
        private readonly ILogger<BooksController> _logger; //Logger service 

        public BooksController(IBookService bookService, ILogger<BooksController> logger) // Injecting the dependicies in the contructor
        {
            _bookService = bookService;
            _logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            try
            {
                _logger.LogInformation("Fetching all books."); //Logging the start of the operation
                var books = await _bookService.GetAllBooksAsync(); // Calls book service to retrieve all books
                if (books == null || !books.Any()) // If no books exist it logs a warning and returns not found message
                {
                    _logger.LogWarning("No books found.");
                    return NotFound("No books found.");
                }
                return Ok(books); // Else returns a list of books with a 200 status code
            }
            catch (Exception ex) // Error handling logging a error if something goes wrong and returns status code 500
            {
                _logger.LogError(ex, "An error occurred while fetching books."); 
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching book with ID {id}"); //Logging the start of the operation
                var book = await _bookService.GetBookByIdAsync(id); // Calls book service to retrieve book by id

                if (book == null) // If book with id is null a warning is logged and not found reposnse is returned
                {
                    _logger.LogWarning($"Book with ID {id} not found.");
                    return NotFound($"Book with ID {id} not found.");
                }

                return Ok(book); // Else returns book with a 200 status code
            }
            catch (Exception ex) // Error handling, logging a and returning a error if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while fetching the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        
        // GET: api/Books/ByGenre/{GenreId} 
        // gets books by genre 
        [HttpGet("ByGenre/{genreId}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBookByGenre(int genreId)
        {
            try
            {
                _logger.LogInformation("Fetching all books."); //Logging the start of the operation
                var books = await _bookService.GetBookByGenreAsync(genreId); //Calls the book service to retrived books by genre id 

                if (books == null || !books.Any()) // If no books are found then a warning is logged and not found response is returned 
                {
                     _logger.LogWarning($"Books with genre ID {genreId} not found.");
                    return NotFound($"Books for genre ID with {genreId} not found.");
                }
                return Ok(books); // Else returns a list of books by genre id with a 200 status code
            }
            catch (Exception ex)
            {
                //Error handling, logging a and returning a error if something goes wrong 
                _logger.LogError(ex, "An error occurred while fetching books for the genre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            try
            {
                if (id != book.BookId) // Checks if id matches the book in Url 
                {
                    _logger.LogWarning("Book ID mismatch."); // If id of the book doesnt match a warning is logged and bad request returned 
                    return BadRequest("Book ID mismatch.");
                }

                await _bookService.UpdateBookAsync(id, book); // Calls book service and updates book information in database
                _logger.LogInformation($"Book with ID {id} updated.");
                return NoContent(); // Returns no content as update was successful
            }
            catch (DbUpdateConcurrencyException)
            { // If book is not found during update a warning is logged and not found is returned
                if (await _bookService.GetBookByIdAsync(id) == null) 
                {
                    _logger.LogWarning($"Book with ID {id} not found for update.");
                    return NotFound($"Book with ID {id} not found."); 
                }
                else
                {
                    _logger.LogError("Error updating book.");  //Logs errro if not found
                    throw; // Rethrows the exception again
                }
            }
            catch (Exception ex) // Error handling, logging a and returning status code 500 if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while updating the book."); 
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")] // Only admin is allowed to create a new book
        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            try
            {
                if (book == null) // If object provided is null 
                {
                    _logger.LogWarning("Received empty book object."); //Warning is logged and bad request response is returned
                    return BadRequest("Book data cannot be null.");
                }

                await _bookService.AddBookAsync(book); // Calls book service and adds book to database 
                _logger.LogInformation($"Book with ID {book.BookId} created."); 
                return CreatedAtAction("GetBook", new { id = book.BookId }, book); //Returns a 201 creation response with the location
            }
            catch (Exception ex) // Error handling, logging a and returning a status code 500 if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while creating the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")] // Only admin is allowed to delete a book
        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
             try
            {
                var book = await _bookService.GetBookByIdAsync(id); // Calls book service to retrive book by id 
                if (book == null) // If book is null
                {
                    _logger.LogWarning($"Book with ID {id} not found."); //warning is logged and bad request response is returned
                    return NotFound($"Book with ID {id} not found."); 
                }

                await _bookService.DeleteBookAsync(id); // Calls service and book is deleted from database 
                _logger.LogInformation($"Book with ID {id} deleted.");
                return NoContent(); // Returns no content as deletion was successful
            }
            catch (Exception ex) // Error handling, logging a and returning a error if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while deleting the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
