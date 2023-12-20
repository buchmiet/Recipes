using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using recipesAPI.Controllers;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;



namespace recipesAPI.Tests.Controllers
{
    [TestFixture]
    public class AuthorControllerTests
    {
        private AuthorController _controller;
        private Mock<IEntityService<Author>> _mockAuthorService;
        private IValidator<CreateAuthorRequest> _validator;

        [SetUp]
        public void Setup()
        {
        
            _mockAuthorService = new Mock<IEntityService<Author>>();
            _validator = new CreateAuthorRequestValidator();

            _controller = new AuthorController(_mockAuthorService.Object, _validator);
        }

        [Test]
        public async Task AddAuthor_ValidData_ReturnsOk()
        {         
            var request = new CreateAuthorRequest
            {
                AuthorName = "Test",
            };
            var author = new Author
            {
                AuthorName = request.AuthorName,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };
            _mockAuthorService.Setup(s => s.AddAsync(It.IsAny<Author>(), It.IsAny<bool>()))
                              .Returns(Task.CompletedTask)
                             .Callback<Author, bool>((a, b) => author.AuthorId = 1);

            var result = await _controller.AddAuthor(request);            
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as AuthorResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(request.AuthorName, Is.EqualTo(response.AuthorName));
            });
        }

        [Test]
        public async Task AddAuthor_NullAuthorName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateAuthorRequest
            {
                AuthorName = null
            };

            // Act
            var result = await _controller.AddAuthor(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddAuthor_EmptyAuthorName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateAuthorRequest
            {
                AuthorName = string.Empty
            };

            // Act
            var result = await _controller.AddAuthor(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }




        [Test]
        public async Task GetAuthor_ExistingAuthor_ReturnsAuthor()
        {
            // Testy dla akcji GetAuthor z kontrolera
            // Przygotowanie danych testowych
            // Wywołanie akcji
            // Aserty na wynik akcji
        }

        // Kontynuuj dla innych akcji CRUD

        // ...
    }

}
