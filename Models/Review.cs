using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace BookReviewApi.Models
{
    public class Review
    {
        public int ReviewId { get; set; } //PK of review 
        
        [Required]
        public string MemberId {get; set; } //FK to member - each review must have a member id 
        
        [Required]
        public int BookId {get; set; } //FK to a book the review is about
        
        [Required] // a rating is required for the review and it must be between 1 to 5
        [Range(1, 5, ErrorMessage = "Rating must be between 1 to 5.")]
        public int Rating {get; set; }
        
        [Required] // a review is required to be between 5 to 300 charcters or error printed
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Review must be be between 5 to 300 characters.")]
        public string ReviewComment { get; set; }
        public DateTime ReviewDate { get; set; } 

        [JsonIgnore]
        public Member? Member { get; set; }  // one to many to member 

        [JsonIgnore]
        public Book? Book { get; set; }  // one to many to book

    }
}