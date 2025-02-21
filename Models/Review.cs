using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookReviewApi.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string MemberId {get; set; }
        public int BookId {get; set; }
        public int Rating {get; set; }
        public string ReviewComment { get; set; }

        [JsonIgnore]
        public Member? Member { get; set; }  // navigational properties

        [JsonIgnore]
        public Book? Book { get; set; }  

    }
}