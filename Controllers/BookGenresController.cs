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
        private readonly IBookGenreService _bookGenreService; // Book service interface 
        private readonly ILogger<BookGenresController> _logger; //Logger service 

        // Injecting the dependicies in the contructor
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
                _logger.LogInformation("Fetching all books."); //Logging the start of the operation
                var bookGenres = await _bookGenreService.GetBookGenresAsync(); // Calls service to retrieve all bookgenres
                if (bookGenres == null || !bookGenres.Any()) // If no bookgenres exist it logs a warning and returns not found message
                {
                    _logger.LogWarning("No bookgenres found.");
                    return NotFound("No bookgenres found.");
                }
                return Ok(bookGenres); // Returns a list of bookgenres

            }
            catch (Exception ex) // Log any exceptions that occur during the process and returns 500 internal error status code 
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
                _logger.LogInformation($"Fetching BookGenre with ID {id}"); // Logging the start of the operation
                var bookGenre = await _bookGenreService.GetBookGenreByIdAsync(id); // Calls service and  bookgenres by id

                if (bookGenre == null) // If it is null a warning is logged and not found reposnse is returned
                {
                    _logger.LogWarning($"Book genre with ID {id} not found.");
                    return NotFound($"Book genre not found.");
                }

                return Ok(bookGenre); // Returns a list of bookgenres 
            }
            catch (Exception ex) // Log any exceptions that occur during the process and returns 500 internal error status code 
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
                if (bookGenre == null) // If bookGenre object is null, log a warning and bad request is returned
                {
                    _logger.LogWarning("Received empty BookGenre object.");
                    return BadRequest("BookGenre data cannot be null.");
                }

                if (id != bookGenre.BookGenreId) // Check if id matches matched URL
                {
                    _logger.LogWarning("BookGenere ID mismatch."); //A warning and bad request is returned is it doesnt match
                    return BadRequest("BookGenre ID mismatch.");
                }

                await _bookGenreService.UpdateBookGenreAsync(id, bookGenre); // Calls service and updates bookgenre in databse
                _logger.LogInformation($"BookGenre with ID {id} updated.");
                return NoContent(); // Returns no content as update was successful
            }
            catch (DbUpdateConcurrencyException) // Exception to log errors and return status code if problem updating the database
            {
                if (await _bookGenreService.GetBookGenreByIdAsync(id) == null) // If bookgenre doesn't exist anymore, warning is logged and vbad request returned 
                {
                    _logger.LogWarning($"BookGenre with ID {id} not found for update.");
                    return NotFound($"BookGenre with ID {id} not found."); 
                }
                else
                {
                    _logger.LogError("Error updating bookgenre."); 
                    throw; // Throws the exception again
                }
            }
            catch (Exception ex) // Log any exceptions that occur during the process and returns 500 internal error status code 
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
                if (bookGenre == null) // if object provided is null, warning is logged bad request is returned 
                {
                    _logger.LogWarning("Received empty BookGenre object.");
                    return BadRequest("BookGenre data cannot be null.");
                }

                await _bookGenreService.AddBookGenreAsync(bookGenre); // Calls service and adds the bookgenre to the database 
                _logger.LogInformation($"BookGenre with ID {bookGenre.BookGenreId} created.");
                return CreatedAtAction("GetBookGenre", new { id = bookGenre.BookGenreId }, bookGenre); //Returns a 201 creation response with the location
            }
            catch (Exception ex) // Log any exceptions that occur during the process and returns 500 internal error status code 
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
                var bookGenre = await _bookGenreService.GetBookGenreByIdAsync(id); // Callsd service and fetches bookgenre by id
                if (bookGenre == null) // if bookgenre not found, logs a warning ad returns not found response
                {
                    _logger.LogWarning($"BookGenre with ID {id} not found.");
                    return NotFound($"BookGenre with ID {id} not found."); 
                }

                await _bookGenreService.DeleteBookGenreAsync(id); // Calls service and bookgenre with id is deleted from database 
                _logger.LogInformation($"BookGenre with ID {id} deleted.");
                return NoContent(); // Returns no content as deletion was successful
            }
            catch (Exception ex) // Log any exceptions that occur during the process and returns 500 internal error status code 
            {
                _logger.LogError(ex, "An error occurred while deleting the BookGenre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
