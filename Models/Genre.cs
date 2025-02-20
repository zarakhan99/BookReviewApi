using System.Collections.Generic;

namespace BookReviewApi.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public List<Book>? Books { get; set; } // navigation property linking genre to books 
    }
}