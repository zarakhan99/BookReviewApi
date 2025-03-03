using Moq;
using Xunit;
using BookReviewApi.Controllers;
using BookReviewApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookReviewApi.Models;
using System.Linq;  
using Microsoft.Extensions.Logging; 

namespace BookReviewApi.UnitTests
{
    public class GetGenreTest
    {
        private readonly Mock<IGenreService> _mockGenreService; // Mock service object
        private readonly GenresController _controller; // Instance of the genre controller 
        private readonly Mock<ILogger<GenresController>> _mockLogger; // Mock logger used in the contorller - log wihtout actual entries

        public GetGenreTest()
        {
            _mockGenreService = new Mock<IGenreService>();
            _mockLogger = new Mock<ILogger<GenresController>>();
            _controller = new GenresController(_mockGenreService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllGenres_ReturnsOkResult_WithListOfGenres()
        {
            var mockGenres = new List<Genre> // List of mock Genre objects 
            {
                new Genre { GenreId = 1, GenreName = "Fantasy" },
                new Genre { GenreId = 2, GenreName = "Romance" }
            };

            //When service method is called - return list of genres 
            _mockGenreService.Setup(service => service.GetAllGenresAsync()).ReturnsAsync(mockGenres); 

            var result = await _controller.GetGenres(); // Calls the controller method to get all genrres

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Genre>>>(result); // Makes sure return tyep is correct - 
            
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result); // Return 200 OK - success
            
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Genre>>(okResult.Value); // Result should cbe a enumerable list
            
            Assert.Equal(2, returnValue.Count()); // Check to see list is number of genre objects we mocked - 2
        }
    }
}

