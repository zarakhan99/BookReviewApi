using Microsoft.EntityFrameworkCore; 
using BookReviewApi.Models;

namespace BookReviewApi.Repositories
{
    public class GenreRepository
    {
        private readonly ApplicationContext _genreContext;

        public GenreRepository(ApplicationContext genreContext)
        {
            _genreContext = genreContext;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync() => await _genreContext.Genres.ToListAsync();

        public async Task<Genre> GetByIdAsync(int id) => await _genreContext.Genres.FindAsync(id);

        public async Task AddAsync(Genre genre)
        {
            _genreContext.Genres.Add(genre);
            await _genreContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Genre genre)
        {
            _genreContext.Entry(genre).State = EntityState.Modified;
            await _genreContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
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
