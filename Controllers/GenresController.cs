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
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService; // genre service interface 
        private readonly ILogger<GenresController> _logger; //logger service 

        public GenresController(IGenreService genreService, ILogger<GenresController> logger) // injecting the dependicies in the contructor  
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
                _logger.LogInformation("Fetching all genres."); 
                var genres = await _genreService.GetAllGenresAsync(); // call genre service to get retrive all genres
                if (genres == null || !genres.Any()) // if no genres exist it logs a warning and returns message
                {
                    _logger.LogWarning("No genres found.");
                    return NotFound("No genres found.");
                }
                return Ok(genres);
            }
            catch (Exception ex) // error handling logging a error if something goes wrong 
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
                _logger.LogInformation($"Fetching genre with ID {id}"); //starts the process 
                var genre = await _genreService.GetGenreByIdAsync(id); // retrives genres that matches id

                if (genre == null) // if it comes up empty a warning is logged and not found reposnse is displayed
                {
                    _logger.LogWarning($"Genre with ID {id} not found.");
                    return NotFound($"Genre with ID {id} not found.");
                }

                return Ok(genre); // otherwise returns genre 
            }
            catch (Exception ex)
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
                if (id != genre.GenreId) // check if id matches 
                {
                    _logger.LogWarning("Genre ID mismatch."); // if id of the genre id doesnt match a warning and bad request is displayed 
                    return BadRequest("Genre ID mismatch.");
                }

                await _genreService.UpdateGenreAsync(id, genre); // updates genre information
                _logger.LogInformation($"Genre with ID {id} updated.");
                return NoContent(); // returns no content as update was successful
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _genreService.GetGenreByIdAsync(id) == null) // if it comes up empty a warning is logged and retunrs not found
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
            catch (Exception ex)
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
                if (genre == null) // if object provided is null bad request is returned 
                {
                    _logger.LogWarning("Received empty genre object.");
                    return BadRequest("Genre data cannot be null.");
                }

                await _genreService.AddGenreAsync(genre); // adds the genre to the database 
                _logger.LogInformation($"Genre with ID {genre.GenreId} created.");
                return CreatedAtAction("GetGenre", new { id = genre.GenreId }, genre);
            }
            catch (Exception ex)
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
                var genre = await _genreService.GetGenreByIdAsync(id); // fecthded genre by id
                if (genre == null)
                {
                    _logger.LogWarning($"Genre with ID {id} not found.");
                    return NotFound($"Genre with ID {id} not found."); 
                }

                await _genreService.DeleteGenreAsync(id); // if found genre with the id is deleted from database 
                _logger.LogInformation($"Genre with ID {id} deleted.");
                return NoContent(); // returns no content as deletion was successful
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the genre.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
