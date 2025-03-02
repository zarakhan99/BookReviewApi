using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models
{
    public class Book
    {
        public int BookId { get; set; } //primary key for each book
        
        [Required]
        public string Title { get; set; } // title of the book - is a required field 

        [Required]
        public string Author { get; set; } //authour of the book - is a required field 

        [Required]
        public int PublishYear { get; set; } //Year the book was published - is a required field 
        
        [Required] // a book description is required and has a limit of 300 characters
        [StringLength(300, MinimumLength = 100, ErrorMessage = "Book Description must be be between 100 to 300 characters.")]
        public string BookDescription {get; set; }
       
       // Navigational Properties
        [JsonIgnore]
        public List<Review>? Reviews { get; set; } // one to many relationship between book and review 
        
        [JsonIgnore]
        public List<BookGenre>? BookGenres { get; set; } // Many to many relationship between book and genre 

    }
}