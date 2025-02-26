using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models
{
    public class GenreDTO
    {
        public int GenreId { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 3)] // A genre name is required and must be between 3 to 100 characters
        public string GenreName { get; set; }
    }
}