using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models
{
    public class BookDTO
    {
       public int BookId { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int PublishYear { get; set; }

        [Required] // a book description is required and has a limit of 300 characters
        [StringLength(300, MinimumLength = 100)]
        public string BookDescription {get; set; }
    }
}