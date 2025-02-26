using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models
{
    public class Book
    {
        public int BookId { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int PublishYear { get; set; }
        
        [Required] // a book description is required and has a limit of 300 characters
        [StringLength(300, MinimumLength = 100, ErrorMessage = "Book Description must be be between 100 to 300 characters.")]
        public string BookDescription {get; set; }
       
       // Navigational Properties
        [JsonIgnore]
        public List<Review>? Reviews { get; set; } // a book can have more than one review 
        
        [JsonIgnore]
        public List<BookGenre>? BookGenres { get; set; } // Many to many relationship between book and genre 

    }
}