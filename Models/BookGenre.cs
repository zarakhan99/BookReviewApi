using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookReviewApi.Models
{
    public class BookGenre
    {
        public int BookGenreId { get; set; } //PK for bookgenre
        public int BookId { get; set; } // book FK each book belongs to a genre 
        public int GenreId { get; set; } // genre FK each book belongs to a genre 
        
         //navigational properties
        [JsonIgnore]
        public Book? Book { get; set; } //junction table - shows many to many relationship to book and genre 
        
        [JsonIgnore]
        public Genre? Genre { get; set; }

    }
}