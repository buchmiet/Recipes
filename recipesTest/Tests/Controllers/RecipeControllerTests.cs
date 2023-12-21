using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using recipesAPI.Controllers;
using recipesCommon.Interfaces;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static recipesApi.DataAccess.RecipesDbContext;
using recipesAPI.Services;

namespace recipesTest.Tests.Controllers
{
    [TestFixture]
    public class RecipeControllerTests
    {
        private RecipeController _controller;
        private Mock<IEntityService<Recipe>> _mockRecipeService;
        private Mock<IEntityService<Author>> _mockAuthorService;
        private Mock<ISearchService> _mockSearchService;
        private IValidator<CreateRecipeRequest> _validator;
        int _validAuthorId = 123;

        [SetUp]
        public void Setup()
        {
            _mockRecipeService = new Mock<IEntityService<Recipe>>();
            _mockAuthorService = new Mock<IEntityService<Author>>();
            _mockSearchService=new Mock<ISearchService>();
            var author = new Author { AuthorId = _validAuthorId };
            _mockAuthorService.Setup(s => s.GetByIdAsync(_validAuthorId))
                             .ReturnsAsync(author);
            _validator = new CreateRecipeRequestValidator(_mockAuthorService.Object);
            _controller = new RecipeController(_mockSearchService.Object,  _mockRecipeService.Object, _validator);
        }

        [Test]
        public async Task AddRecipe_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateRecipeRequest
            {
                Title = "TestRecipe",
                CookingTime = 30, 
                AuthorId = _validAuthorId
            };
            var recipe = new Recipe
            {
                Title = request.Title,
                CookingTime = request.CookingTime,
                AuthorId = request.AuthorId,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };
            _mockRecipeService.Setup(s => s.AddAsync(It.IsAny<Recipe>(), It.IsAny<bool>()))
                              .Returns(Task.CompletedTask)
                              .Callback<Recipe, bool>((a, b) => recipe.RecipeId = 1);

            // Act
            var result = await _controller.AddRecipe(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as RecipeResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Title, Is.EqualTo(request.Title));
                Assert.That(response.CookingTime, Is.EqualTo(request.CookingTime));
                Assert.That(response.AuthorId, Is.EqualTo(request.AuthorId));
               
            });
        }

        [Test]
        public async Task AddRecipe_NullTitle_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeRequest
            {
                Title = null,
                CookingTime = 30,
                AuthorId = _validAuthorId
            };

            // Act
            var result = await _controller.AddRecipe(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipe_EmptyTitle_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeRequest
            {
                Title = string.Empty,
                CookingTime = 30,
                AuthorId = _validAuthorId
            };

            // Act
            var result = await _controller.AddRecipe(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipe_CookingTimeLessThanOne_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeRequest
            {
                Title = "ValidTitle",
                CookingTime = 0, 
                AuthorId = _validAuthorId
            };

            // Act
            var result = await _controller.AddRecipe(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipe_InvalidAuthorId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeRequest
            {
                Title = "ValidTitle",
                CookingTime = 1,
                AuthorId = 1
            };

            // Act
            var result = await _controller.AddRecipe(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

    }
}
