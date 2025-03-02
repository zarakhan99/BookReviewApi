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
    [Route("api/[controller]")] // The controller's base URL will be api/Genres
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService; // genre service interface 
        private readonly ILogger<GenresController> _logger; //Logger service to log messages for debugging 

         // Injecting the dependicies in the contructor 
        public GenresController(IGenreService genreService, ILogger<GenresController> logger) 
        {
            _genreService = genreService;
            _logger = logger;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            try
            {
                _logger.LogInformation("Fetching all genres."); //logging the start of the operation
                var genres = await _genreService.GetAllGenresAsync(); // call genre service to get retrive all genres
                if (genres == null || !genres.Any()) // if no genres exist it logs a warning and returns a not found message 
                {
                    _logger.LogWarning("No genres found.");
                    return NotFound("No genres found.");
                }
                return Ok(genres); // Else returns a list of genres with a 200 status code
            }
            catch (Exception ex) // Error handling logging a error if something goes wrong and returns status code 500
            {
                _logger.LogError(ex, "An error occurred while fetching genres."); //
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching genre with ID {id}"); //Logging the start of the operation
                var genre = await _genreService.GetGenreByIdAsync(id); // Retrives genres that matches the id

                if (genre == null) // If it comes up empty a warning is logged and not found reposnse is displayed with the id
                {
                    _logger.LogWarning($"Genre with ID {id} not found.");
                    return NotFound($"Genre with ID {id} not found.");
                }

                return Ok(genre); // Otherwise returns the genre with a 200 status code
            }
            catch (Exception ex) // Error handling, logging a and returning a error if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while fetching the genre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")] // only admins can update a genre 
        // PUT: api/Genres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, Genre genre)
        {
            try
            {
                if (id != genre.GenreId) // check if id matches to a genre
                {
                    _logger.LogWarning("Genre ID mismatch."); // if id of the genre doesnt match, a bad request is returned
                    return BadRequest("Genre ID mismatch.");
                }

                await _genreService.UpdateGenreAsync(id, genre); // Genre service method - Updates genre information in database
                _logger.LogInformation($"Genre with ID {id} updated."); //Logs which genre has been updated 
                return NoContent(); // Returns 204 no content as update was successful
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _genreService.GetGenreByIdAsync(id) == null) // If it comes up empty a warning is logged and retunrs not found
                {
                    _logger.LogWarning($"Genre with ID {id} not found for update.");
                    return NotFound($"Genre with ID {id} not found."); 
                }
                else
                {
                    _logger.LogError("Error updating Genre."); 
                    throw; // throws the exception again
                }
            }
            catch (Exception ex) // Error handling, logging a and returning a error if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while updating the Genre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


        [Authorize(Roles = "Admin")] // only admins can create a genre 
        // POST: api/Genres
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            try
            {
                if (genre == null) // If object provided is null a warning is logged and a bad request is returned 
                {
                    _logger.LogWarning("Received empty genre object.");
                    return BadRequest("Genre data cannot be null.");
                }

                await _genreService.AddGenreAsync(genre); // Calls the genre service and adds the genre to the database 
                _logger.LogInformation($"Genre with ID {genre.GenreId} created.");
                return CreatedAtAction("GetGenre", new { id = genre.GenreId }, genre); //Returns a 201 creation response with the location
            }
            catch (Exception ex) // Error handling, logging a and returning a error if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while creating the genre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")] // only admins casn delete a genre 
        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            try
            {
                var genre = await _genreService.GetGenreByIdAsync(id); // Fetch genre by id using service method
                if (genre == null) // If genre is not found, a warning is logged and not found response is returned 
                {
                    _logger.LogWarning($"Genre with ID {id} not found.");
                    return NotFound($"Genre with ID {id} not found."); 
                }

                await _genreService.DeleteGenreAsync(id); // If found, genre with the id is deleted from database 
                _logger.LogInformation($"Genre with ID {id} deleted.");
                return NoContent(); // Returns no content as deletion was successful
            }
            catch (Exception ex) // Error handling, logging a and returning a error if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while deleting the genre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
