using System.Collections.Generic;

namespace BookReviewApi.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int MemberId {get; set; }
        public int BookId {get; set; }
        public int Rating {get; set; }
        public string ReviewDescription { get; set; }
        public DateTime ReviewDate {get; set; }

        public Member Member { get; set; }  // Links to Member
        public Book Book { get; set; }  

    }
}