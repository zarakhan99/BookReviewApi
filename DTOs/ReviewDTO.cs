using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        
        [Required]
        public string MemberId {get; set; }
        
        [Required]
        public int BookId {get; set; }
        
        [Required] // a rating is required for the review and it must be between 1 to 5
        [Range(1, 5)]
        public int Rating {get; set; }

        [Required] // a review is required and be between 5 and 300 characters
        [StringLength(300, MinimumLength = 5)]
        public string ReviewComment { get; set; }
        public DateTime ReviewDate { get; set; } 
    }
}