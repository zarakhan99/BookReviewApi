using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace BookReviewApi.Models
{
    public class Member : IdentityUser
    {

        [JsonIgnore]
        public List<Review>? Reviews { get; set; } //navigational properties 
    }
}