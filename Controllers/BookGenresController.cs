using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookReviewApi.Models;

namespace BookReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookGenresController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public BookGenresController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/BookGenres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookGenre>>> GetBookGenres()
        {
            return await _context.BookGenres.ToListAsync();
        }

        // GET: api/BookGenres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookGenre>> GetBookGenre(int id)
        {
            var bookGenre = await _context.BookGenres.FindAsync(id);

            if (bookGenre == null)
            {
                return NotFound();
            }

            return bookGenre;
        }

        // PUT: api/BookGenres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookGenre(int id, BookGenre bookGenre)
        {
            if (id != bookGenre.BookGenreId)
            {
                return BadRequest();
            }

            _context.Entry(bookGenre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookGenreExists(id))
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

        // POST: api/BookGenres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookGenre>> PostBookGenre(BookGenre bookGenre)
        {
            _context.BookGenres.Add(bookGenre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookGenre", new { id = bookGenre.BookGenreId }, bookGenre);
        }

        // DELETE: api/BookGenres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookGenre(int id)
        {
            var bookGenre = await _context.BookGenres.FindAsync(id);
            if (bookGenre == null)
            {
                return NotFound();
            }

            _context.BookGenres.Remove(bookGenre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookGenreExists(int id)
        {
            return _context.BookGenres.Any(e => e.BookGenreId == id);
        }
    }
}
