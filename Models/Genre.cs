using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models
{
    public class Genre
    {
        public int GenreId { get; set; } //PK for genre 
        
        [Required] // Required fired and error message prinied out if genre name exceeds length
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Genre name must be be between 3 to 100 characters.")]
        public string GenreName { get; set; } 

        [JsonIgnore]
        public List<BookGenre>? BookGenres { get; set; } // Many to many relationship between book and genre 
    }
}