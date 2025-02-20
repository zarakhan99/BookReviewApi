using System.Collections.Generic;

namespace BookReviewApi.Models
{
    public class Member
    {
        public int MemberId { get; set; } // primary key
        public string Name { get; set; }
        public List<Review>? Reviews { get; set; } //navigational properties 
        public List<Bookmark>? Bookmarks { get; set; }
    }
}