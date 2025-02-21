using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookReviewApi.Models
{
    public class Member 
    {
        [JsonIgnore]
        public List<Review>? Reviews { get; set; } //navigational properties 
    }
}