using System.Collections.Generic;

namespace BookReviewApi.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishYear { get; set; }
        public string BookDescription {get; set; }
        public int GenreId { get; set; } // genre FK each book belongs to a genre 
        public List<Review>? Reviews { get; set; } //For Navigational Proprties

    }
}