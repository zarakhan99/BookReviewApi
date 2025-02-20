using System.Collections.Generic;

namespace BookReviewApi.Models
{
    public class Bookmark
    {
        public int BookmarkId { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }

        public Member Member { get; set; }  // Navigational properties
        public Book Book { get; set; } 
    }
}