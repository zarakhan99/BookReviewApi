using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookReviewApi.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishYear { get; set; }
        public string BookDescription {get; set; }
       
       [JsonIgnore]
        public List<Review>? Reviews { get; set; } //For Navigational Proprties
        
        [JsonIgnore]
        public List<BookGenre>? BookGenres { get; set; }

    }
}