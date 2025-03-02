using Microsoft.EntityFrameworkCore; 
using BookReviewApi.Context;
using BookReviewApi.Models;

// Genre repository to act as a layer between the databse and controllers

namespace BookReviewApi.Repositories
{
    public class GenreRepository
    {
        private readonly ApplicationContext _genreContext; // Aplication context to interact with the book table

        public GenreRepository(ApplicationContext genreContext) // Contructor to inject depenendency and initialise database context
        {
            _genreContext = genreContext;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync() => await _genreContext.Genres.ToListAsync(); // Retrieves list of genres 

        public async Task<Genre> GetByIdAsync(int id) => await _genreContext.Genres.FindAsync(id);  // Finds genre entity with the same id in database 

        public async Task AddAsync(Genre genre) // Adds a genre to the database
        {
            _genreContext.Genres.Add(genre);
            await _genreContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Genre genre) // Updates genre entity
        {
            _genreContext.Entry(genre).State = EntityState.Modified;
            await _genreContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) // Deletes a genre entity with matching id
        {
            var genre = await _genreContext.Genres.FindAsync(id);
            if (genre != null)
            {
                _genreContext.Genres.Remove(genre);
                await _genreContext.SaveChangesAsync();
            }
        }
    }
}
