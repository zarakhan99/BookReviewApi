using Moq;
using Xunit;
using BookReviewApi.Controllers;
using BookReviewApi.Services;
using Microsoft.AspNetCore.Mvc;
using BookReviewApi.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

//Unit testting the postgenre controller method in genrecontroller

namespace BookReviewApi.UnitTests
{
    public class AddGenreTest
    {
        private readonly Mock<IGenreService> _mockGenreService; // mock service object
        private readonly GenresController _controller; // Instance of the genre controller 
        private readonly Mock<ILogger<GenresController>> _mockLogger; // mock logger used in the contorller - log wihtout actual entries

        public AddGenreTest()
        {
            _mockGenreService = new Mock<IGenreService>();
            _mockLogger = new Mock<ILogger<GenresController>>();
            _controller = new GenresController(_mockGenreService.Object, _mockLogger.Object);
        }

        // Mocking Admin
        private void MockAdminRole() // Mock admin role used for a mock authecticated user 
        {
            var claims = new[] // Creating a claim - inofmration about a user
            {
                new Claim(ClaimTypes.Name, "adminUser"),
                new Claim(ClaimTypes.Role, "Admin") // Assigning admin role 
            };
        
            var identity = new ClaimsIdentity(claims, "mock"); // Identity object - user in the system 
            var user = new ClaimsPrincipal(identity);
        
            var context = new DefaultHttpContext();
            context.User = user; // User is assigned admin
            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = context // http asisigned to controller context - mocking a request to the controller
            };
        }

        
        [Fact] // Marks the method as a test used by xunit
        public async Task PostGenre_ReturnsCreatedAtAction()
        {
            
            MockAdminRole(); // Admin user 

            var mockGenre = new Genre 
            { 
                GenreName = "Fantasy"  // Mock genre object we want to create
            }; 

            var result = await _controller.PostGenre(mockGenre); // call to controller method to add the genre object

            var actionResult = Assert.IsType<ActionResult<Genre>>(result); 

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result); // Check if return type is created at action and returns 201

            Assert.Equal("GetGenre", createdAtActionResult.ActionName); // If correct we retrive the genre
            Assert.IsType<Genre>(createdAtActionResult.Value); //Verifys for last time it is a genre
        }
    }
}

