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

namespace recipesTest.Tests.Controllers
{
    [TestFixture]
    public class TagControllerTests
    {
        private TagController _controller;
        private Mock<IEntityService<Tag>> _mockTagService;
        private IValidator<CreateTagRequest> _validator;

        [SetUp]
        public void Setup()
        {
            _mockTagService = new Mock<IEntityService<Tag>>();
            _validator = new CreateTagRequestValidator(_mockTagService.Object);

            _controller = new TagController(_mockTagService.Object, _validator);
        }

        [Test]
        public async Task AddTag_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateTagRequest
            {
                Name = "TestTag",
            };
            var tag = new Tag
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow
            };
            _mockTagService.Setup(s => s.AddAsync(It.IsAny<Tag>(), It.IsAny<bool>()))
                            .Returns(Task.CompletedTask)
                           .Callback<Tag,bool>((a,b) => tag.TagId = 1);
                          

            // Act
            var result = await _controller.AddTag(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as TagResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(request.Name, Is.EqualTo(response.Name));
            });
        }

        [Test]
        public async Task AddTag_NullTagName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateTagRequest
            {
                Name = null
            };

            // Act
            var result = await _controller.AddTag(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddTag_EmptyTagName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateTagRequest
            {
                Name = string.Empty
            };

            // Act
            var result = await _controller.AddTag(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]  
        public async Task AddTag_WithExistingName_ReturnsBadRequest()
        {
            // Arrange
            var existingName = "Winter Food";
            var request = new CreateTagRequest { Name = existingName };
            var existingTag = new Tag { TagId = 1, Name = existingName };

            _mockTagService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<Tag, bool>>>(), It.IsAny<Expression<Func<Tag, object>>>()))
                .Returns((Expression<Func<Tag, bool>> predicate, Expression<Func<Tag, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingTag) ? existingTag : null);
                });

            // Act
            var result = await _controller.AddTag(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

    }

}
