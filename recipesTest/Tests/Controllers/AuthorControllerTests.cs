using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using recipesAPI.Controllers;
using recipesCommon.Interfaces;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using System.Linq.Expressions;
using static recipesApi.DataAccess.RecipesDbContext;



namespace recipesAPI.Tests.Controllers
{
    [TestFixture]
    public class AuthorControllerTests
    {
        private AuthorController _controller;
        private Mock<IEntityService<Author>> _mockAuthorService;
        private IValidator<CreateAuthorRequest> _validator;
      

        [SetUp]
        public  void Setup()
        {
        
            _mockAuthorService = new Mock<IEntityService<Author>>();
            _mockAuthorService = new Mock<IEntityService<Author>>();
            _validator = new CreateAuthorRequestValidator(_mockAuthorService.Object);
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
        public async Task AddAuthor_WithExistingName_ReturnsBadRequest()
        {
            // Arrange
            var _ExistingAuthorName = "ExistingAuthor";
            var request = new CreateAuthorRequest { AuthorName = _ExistingAuthorName };
            var existingAuthor = new Author { AuthorId = 1, AuthorName = _ExistingAuthorName };

            _mockAuthorService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<Author, bool>>>(), It.IsAny<Expression<Func<Author, object>>>()))
                   .Returns((Expression<Func<Author, bool>> predicate, Expression<Func<Author, object>> orderByDescending) =>
                   {
                       var compiledPredicate = predicate.Compile();
                       return Task.FromResult(compiledPredicate(existingAuthor) ? existingAuthor : null);
                   });
          
            // Act
            var result = await _controller.AddAuthor(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

    }

}
