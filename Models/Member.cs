using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace BookReviewApi.Models
{
    public class Member : IdentityUser //inherits identity user to provide built in properties
    {

        [JsonIgnore]
        public List<Review>? Reviews { get; set; } //navigational properties 
    }
}