using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        [Required]
        public string GenreName { get; set; }

        [JsonIgnore]
        public List<BookGenre>? BookGenres { get; set; } // Many to many relationship between book and genre 
    }
}