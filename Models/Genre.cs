using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookReviewApi.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }

        [JsonIgnore]
        public List<BookGenre>? BookGenres { get; set; } // navigation property linking genre to books 
    }
}