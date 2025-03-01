using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; 
using BookReviewApi.Models;

namespace BookReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService; // genre service interface 
        private readonly ILogger<ReviewsController> _logger; //logger service 

        public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
        {
            _reviewService = reviewService;
            _logger = logger;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            try
            {
                _logger.LogInformation("Fetching all reviews."); 
                var reviews = await _reviewService.GetAllReviewsAsync(); // calls review service to get retrive all reviews
                if (reviews == null || !reviews.Any()) // if no reviews exist it logs a warning and returns message
                {
                    _logger.LogWarning("No reviews found.");
                    return NotFound("No reviews found.");
                }
                return Ok(reviews);
            }
            catch (Exception ex) // error handling logging a error if something goes wrong 
            {
                _logger.LogError(ex, "An error occurred while fetching reviews."); 
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching review with ID {id}"); //starts the process 
                var review = await _reviewService.GetReviewByIdAsync(id); // retrives genres that matches id

                if (review == null) // if it comes up empty a warning is logged and not found reposnse is displayed
                {
                    _logger.LogWarning($"Review with ID {id} not found.");
                    return NotFound($"Review with ID {id} not found.");
                }

                return Ok(review); // otherwise returns genre 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the review.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("ByBook/{bookId}")]
        public async Task<ActionResult<Book>> GetReviewsForBook(int bookId)
        {
            try
            {
                _logger.LogInformation("Fetching all reviews."); 
                var reviews = await _reviewService.GetReviewsForBookAsync(bookId);

                if (reviews ==null || !reviews.Any()) // if no reviews are found then a not found error is returned 
                {
                     _logger.LogWarning($"Reviews for book ID {bookId} not found.");
                    return NotFound($"Review for book ID with {bookId} not found.");
                }
                return Ok(reviews); 

            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the process
                _logger.LogError(ex, "An error occurred while fetching reviews for the book.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
            
        }

        [Authorize(Roles = "Admin")]
        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
           if (id != review.ReviewId)
            {
                _logger.LogWarning("Review ID mismatch.");
                return BadRequest("Review ID mismatch."); 
            }
            
            var exReview = await _reviewService.GetReviewByIdAsync(id);
            
            if (exReview == null)
            {
                _logger.LogWarning($"Review with ID {id} not found for update.");
                return NotFound($"Review with ID {id} not found.");
            }
            
            exReview.Rating = review.Rating;
            exReview.ReviewComment = review.ReviewComment;
            exReview.ReviewDate = DateTime.UtcNow; 
            
            try
            {
                await _reviewService.UpdateReviewAsync(id, exReview);
                _logger.LogInformation($"Review with ID {id} updated.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogError($"An error occurred while updating the review.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating review.");
            }
        }

        [Authorize]
        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            try
            {
                if (review == null) // if object provided is null bad request is returned 
                {
                    _logger.LogWarning("Received empty review object.");
                    return BadRequest("Review data cannot be null.");
                }

                review.ReviewDate = DateTime.UtcNow;

                await _reviewService.AddReviewAsync(review); // adds the genre to the database 
                _logger.LogInformation($"Review with ID {review.ReviewId} created.");
                return CreatedAtAction("GetReview", new { id = review.ReviewId }, review);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the review.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        
        [Authorize(Roles = "Admin")]
         // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id); // fetch review by id
                if (review == null)
                {
                    _logger.LogWarning($"Review with ID {id} not found.");
                    return NotFound($"Review with ID {id} not found."); 
                }

                await _reviewService.DeleteReviewAsync(id); // if found, review with the id is deleted from database 
                _logger.LogInformation($"Review with ID {id} deleted.");
                return NoContent(); // returns no content as deletion was successful
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the review.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
