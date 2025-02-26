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
    [Authorize(Roles = "Admin")]
    public class BookGenresController : ControllerBase
    {
        private readonly IBookGenreService _bookGenreService; // genre service interface 
        private readonly ILogger<BookGenresController> _logger; //logger service 

        public BookGenresController(IBookGenreService bookGenreService, ILogger<BookGenresController> logger)
        {
            _bookGenreService = bookGenreService;
            _logger = logger;
        }

        // GET: api/BookGenres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookGenre>>> GetBookGenres()
        {
            try
            {
                _logger.LogInformation("Fetching all books."); 
                var bookGenres = await _bookGenreService.GetBookGenresAsync(); // calls review service to get retrive all reviews
                if (bookGenres == null || !bookGenres.Any()) // if no reviews exist it logs a warning and returns message
                {
                    _logger.LogWarning("No bookgenres found.");
                    return NotFound("No bookgenres found.");
                }
                return Ok(bookGenres);
            }
            catch (Exception ex) // error handling logging a error if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while fetching book genres."); 
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/BookGenres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookGenre>> GetBookGenre(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching BookGenre with ID {id}"); //starts the process 
                var bookGenre = await _bookGenreService.GetBookGenreByIdAsync(id); // retrives genres that matches id

                if (bookGenre == null) // if it comes up empty a warning is logged and not found reposnse is displayed
                {
                    _logger.LogWarning($"Book genre with ID {id} not found.");
                    return NotFound($"Book genre not found.");
                }

                return Ok(bookGenre); // otherwise returns genre 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the bookgenre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/BookGenres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookGenre(int id, BookGenre bookGenre)
        {
            try
            {
                if (bookGenre == null)
                {
                    _logger.LogWarning("Received empty BookGenre object.");
                    return BadRequest("BookGenre data cannot be null.");
                }

                if (id != bookGenre.BookGenreId) // checks if id matches 
                {
                    _logger.LogWarning("BookGenere ID mismatch."); // if id of the book id doesnt match a warning and bad request is displayed 
                    return BadRequest("BookGenre ID mismatch.");
                }

                await _bookGenreService.UpdateBookGenreAsync(id, bookGenre); // updates book information
                _logger.LogInformation($"BookGenre with ID {id} updated.");
                return NoContent(); // returns no content as update was successful
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _bookGenreService.GetBookGenreByIdAsync(id) == null) // if it comes up empty a warning is logged and retunrs not found
                {
                    _logger.LogWarning($"BookGenre with ID {id} not found for update.");
                    return NotFound($"BookGenre with ID {id} not found."); 
                }
                else
                {
                    _logger.LogError("Error updating bookgenre."); 
                    throw; // throws the exception again
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the bookgenre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/BookGenres
        [HttpPost]
        public async Task<ActionResult<BookGenre>> PostBookGenre(BookGenre bookGenre)
        {
            try
            {
                if (bookGenre == null) // if object provided is null bad request is returned 
                {
                    _logger.LogWarning("Received empty BookGenre object.");
                    return BadRequest("BookGenre data cannot be null.");
                }

                await _bookGenreService.AddBookGenreAsync(bookGenre); // adds the genre to the database 
                _logger.LogInformation($"BookGenre with ID {bookGenre.BookGenreId} created.");
                return CreatedAtAction("GetBookGenre", new { id = bookGenre.BookGenreId }, bookGenre);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the genre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/BookGenres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookGenre(int id)
        {
            try
            {
                var bookGenre = await _bookGenreService.GetBookGenreByIdAsync(id); // fecthded genre by id
                if (bookGenre == null)
                {
                    _logger.LogWarning($"BookGenre with ID {id} not found.");
                    return NotFound($"BookGenre with ID {id} not found."); 
                }

                await _bookGenreService.DeleteBookGenreAsync(id); // if found genre with the id is deleted from database 
                _logger.LogInformation($"BookGenre with ID {id} deleted.");
                return NoContent(); // returns no content as deletion was successful
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the BookGenre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
