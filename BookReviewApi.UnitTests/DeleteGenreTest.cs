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

//Unit testting the DeleteGenre controller method in genrecontroller

namespace BookReviewApi.UnitTests
{
    public class DeleteGenreTest
    {
        private readonly Mock<IGenreService> _mockGenreService; // mock service object
        private readonly GenresController _controller; // Instance of the genre controller 
        private readonly Mock<ILogger<GenresController>> _mockLogger; // mock logger used in the contorller - log wihtout actual entries

        public DeleteGenreTest()
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
                new Claim(ClaimTypes.Role, "Admin") // Assing mock role name 
            };
        
            var identity = new ClaimsIdentity(claims, "mock");
            var user = new ClaimsPrincipal(identity);
        
            var context = new DefaultHttpContext();
            context.User = user; // User is assigned admin
            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = context // http asisigned to controller context - mocking a request to the controller
            };
        }

        [Theory] // Allows you to input mulyiple parameters 
        [InlineData(1)] // Value of parameter we are inputting 
        public async Task DeleteGenreId_ReturnsNoContent(int id)
        {
            
            var mockGenre = new Genre
            {
                GenreId = id,
                GenreName = "Crime" // Mock genre object we want to create
            };

            MockAdminRole(); // Admin user 

            _mockGenreService.Setup(service => service.GetGenreByIdAsync(id)) //Mock controller method to find genre by id
            .ReturnsAsync(mockGenre); // returns the mock genre object

            _mockGenreService.Setup(service => service.DeleteGenreAsync(id)) //mocks the delete method from controller 
            .Returns(Task.CompletedTask);  // When method is called return a completed task

            var result = await _controller.DeleteGenre(id); // call to controller method to delete the genre object

            var noContentResult = Assert.IsType<NoContentResult>(result); // checks response is correct - 204 no content

            _mockGenreService.Verify(service => service.GetGenreByIdAsync(id), Times.Once); // makes sure controller methods are called once
            _mockGenreService.Verify(service => service.DeleteGenreAsync(id), Times.Once);
        }
    }
}

