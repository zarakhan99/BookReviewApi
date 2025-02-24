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
        private readonly ApplicationContext _context;

        public ReviewsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpGet("ByBook/{bookId}")]
        public async Task<ActionResult<Book>> GetReviewsForBook(int bookId)
        {
            var reviews = await _context.Reviews
            .Where(r => r.BookId == bookId) //checks if reviews are associated with a specific book id
            .ToListAsync();
            
            if (reviews.Count == 0) // if no reviews are found then a not found error is returned 
            {
                return NotFound();
            }
            return Ok(reviews); 
        }

        [Authorize]
        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                return BadRequest();
            }

             var appUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             var admin = User.IsInRole("Admin");
             var owner = review.MemberId == appUser;

             if(appUser == null)
             {
                return Unauthorized();
             }

             if (!admin && !owner)
             {
                return Forbid(); 
             }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize]
        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            review.ReviewDate = DateTime.UtcNow; //Ensures the date and time are automatically created with a universal time  
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.ReviewId }, review);
        }
        
        
        [Authorize]
         // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

             var appUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             var admin = User.IsInRole("Admin");
             var owner = review.MemberId == appUser;

             if(appUser == null)
             {
                return Unauthorized();
             }

             if (!admin && !owner)
             {
                return Forbid(); 
             }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
