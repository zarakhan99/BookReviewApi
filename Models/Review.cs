using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string MemberId {get; set; }
        public int BookId {get; set; }
        
        [Required] // a rating is required for the review and it must be between 1 to 5
        [Range(1, 5, ErrorMessage = "Rating must be between 1 to 5.")]
        public int Rating {get; set; }

        [Required] // a review is required and be more than 300 characters
        [MaxLength(300, ErrorMessage = "Review cannot be greater than 300 characters.")]
        public string ReviewComment { get; set; }
        public DateTime ReviewDate { get; set; } 

        [JsonIgnore]
        public Member? Member { get; set; }  // navigational properties

        [JsonIgnore]
        public Book? Book { get; set; }  

    }
}