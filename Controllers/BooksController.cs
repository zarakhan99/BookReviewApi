using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BookReviewApi.Models;

namespace BookReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService; // genre service interface 
        private readonly ILogger<BooksController> _logger; //logger service 

        public BooksController(IBookService bookService, ILogger<BooksController> logger)
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
                _logger.LogInformation("Fetching all books."); 
                var books = await _bookService.GetAllBooksAsync(); // calls review service to get retrive all reviews
                if (books == null || !books.Any()) // if no reviews exist it logs a warning and returns message
                {
                    _logger.LogWarning("No books found.");
                    return NotFound("No books found.");
                }
                return Ok(books);
            }
            catch (Exception ex) // error handling logging a error if something goes wrong 
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
                _logger.LogInformation($"Fetching book with ID {id}"); //starts the process 
                var book = await _bookService.GetBookByIdAsync(id); // retrives genres that matches id

                if (book == null) // if it comes up empty a warning is logged and not found reposnse is displayed
                {
                    _logger.LogWarning($"Book with ID {id} not found.");
                    return NotFound($"Book with ID {id} not found.");
                }

                return Ok(book); // otherwise returns genre 
            }
            catch (Exception ex)
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
                _logger.LogInformation("Fetching all books."); 
                var books = await _bookService.GetBookByGenreAsync(genreId);

                if (books == null || !books.Any()) // if no books are found then a not found error is returned 
                {
                     _logger.LogWarning($"Books with genre ID {genreId} not found.");
                    return NotFound($"Books for genre ID with {genreId} not found.");
                }
                return Ok(books); 
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the process
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
                if (id != book.BookId) // checks if id matches 
                {
                    _logger.LogWarning("Book ID mismatch."); // if id of the book id doesnt match a warning and bad request is displayed 
                    return BadRequest("Book ID mismatch.");
                }

                await _bookService.UpdateBookAsync(id, book); // updates book information
                _logger.LogInformation($"Book with ID {id} updated.");
                return NoContent(); // returns no content as update was successful
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _bookService.GetBookByIdAsync(id) == null) // if it comes up empty a warning is logged and retunrs not found
                {
                    _logger.LogWarning($"Book with ID {id} not found for update.");
                    return NotFound($"Book with ID {id} not found."); 
                }
                else
                {
                    _logger.LogError("Error updating book."); 
                    throw; // throws the exception again
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")] // only admin are allowed to create a new book
        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            try
            {
                if (book == null) // if object provided is null bad request is returned 
                {
                    _logger.LogWarning("Received empty book object.");
                    return BadRequest("Book data cannot be null.");
                }

                await _bookService.AddBookAsync(book); // adds the genre to the database 
                _logger.LogInformation($"Book with ID {book.BookId} created.");
                return CreatedAtAction("GetBook", new { id = book.BookId }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
             try
            {
                var book = await _bookService.GetBookByIdAsync(id); // fecthded genre by id
                if (book == null)
                {
                    _logger.LogWarning($"Book with ID {id} not found.");
                    return NotFound($"Book with ID {id} not found."); 
                }

                await _bookService.DeleteBookAsync(id); // if found genre with the id is deleted from database 
                _logger.LogInformation($"Book with ID {id} deleted.");
                return NoContent(); // returns no content as deletion was successful
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
