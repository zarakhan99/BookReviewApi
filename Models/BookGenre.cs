using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookReviewApi.Models
{
    public class BookGenre
    {
        public int BookGenreId { get; set; }
        public int BookId { get; set; }
        public int GenreId { get; set; } // genre FK each book belongs to a genre 
        
        [JsonIgnore]
        public Book? Book { get; set; } //navigational properties
        
        [JsonIgnore]
        public Genre? Genre { get; set; }

    }
}