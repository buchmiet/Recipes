using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using recipesAPI.Controllers;
using recipesCommon.Interfaces;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesTest.Tests.Controllers
{
    [TestFixture]
    public class PhotoRecipeControllerTests
    {
        private PhotoRecipeController _controller;
        private Mock<IEntityService<PhotoRecipe>> _mockPhotoRecipeService;
        private Mock<IEntityService<Photo>> _mockPhotoService;
        private Mock<IEntityService<Recipe>> _mockRecipeService;
        private IValidator<CreatePhotoRecipeRequest> _validator;
        private int _validPhotoId = 30;
        private int _validRecipeId = 40;

        [SetUp]
        public void Setup()
        {
            _mockPhotoService = new Mock<IEntityService<Photo>>();
            _mockRecipeService = new Mock<IEntityService<Recipe>>();

            var photo = new Photo { PhotoId = _validPhotoId };
            var recipe = new Recipe { RecipeId = _validRecipeId };

            _mockPhotoService.Setup(s => s.GetByIdAsync(_validPhotoId))
                             .ReturnsAsync(photo);
            _mockRecipeService.Setup(r => r.GetByIdAsync(_validRecipeId))
                              .ReturnsAsync(recipe);

            _mockPhotoRecipeService = new Mock<IEntityService<PhotoRecipe>>();

            _validator = new CreatePhotoRecipeRequestValidator(_mockPhotoService.Object, _mockRecipeService.Object, _mockPhotoRecipeService.Object);

            _controller = new PhotoRecipeController(_mockPhotoRecipeService.Object, _validator);
        }

        [Test]
        public async Task AddPhotoRecipe_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreatePhotoRecipeRequest
            {
                PhotoId = _validPhotoId,
                RecipeId = _validRecipeId,        
            };

            var photoRecipe = new PhotoRecipe
            {
                PhotoId = request.PhotoId,
                RecipeId = request.RecipeId,             
                CreatedOn = DateTime.UtcNow
            };

            _mockPhotoRecipeService.Setup(s => s.AddAsync(It.IsAny<PhotoRecipe>(), It.IsAny<bool>()))
                                   .Returns(Task.CompletedTask)
                                   .Callback<PhotoRecipe, bool>((pr, b) => pr.PhotoRecipeId = 1);

            // Act
            var result = await _controller.AddPhotoRecipe(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as PhotoRecipeResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.PhotoId, Is.EqualTo(request.PhotoId));
                Assert.That(response.RecipeId, Is.EqualTo(request.RecipeId));            
            });
        }


        [Test]
     
        public async Task AddPhotoRecipe_WhenNoExistingEntries_SetsPositionToZero()
        {
            // Arrange
            var request = new CreatePhotoRecipeRequest
            {
                PhotoId = _validPhotoId,
                RecipeId = _validRecipeId
            };

            var emptyPhotoRecipes = new List<PhotoRecipe>().AsQueryable();
            _mockPhotoRecipeService.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<PhotoRecipe, bool>>>()))
                           .ReturnsAsync((Expression<Func<PhotoRecipe, bool>> predicate) => emptyPhotoRecipes.Where(predicate.Compile()).AsQueryable());


            // Act
            var result = await _controller.AddPhotoRecipe(request);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as PhotoRecipeResponse;
            Assert.That(response.Position, Is.EqualTo(0));
        }

        [Test]
        public async Task AddPhotoRecipe_WhenOneExistingEntry_SetsPositionToOne()
        {
            // Arrange
            var request = new CreatePhotoRecipeRequest
            {
                PhotoId = _validPhotoId,
                RecipeId = _validRecipeId
            };

            var existingPhotoRecipes = new List<PhotoRecipe>
    {
        new PhotoRecipe { RecipeId = _validRecipeId, Position = 0 }
    }.AsQueryable();

            _mockPhotoRecipeService.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<PhotoRecipe, bool>>>()))
                           .ReturnsAsync((Expression<Func<PhotoRecipe, bool>> predicate) => existingPhotoRecipes.Where(predicate.Compile()).AsQueryable());


      

            // Act
            var result = await _controller.AddPhotoRecipe(request);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as PhotoRecipeResponse;
            Assert.That(response.Position, Is.EqualTo(1));
        }


        [Test]
        public async Task AddPhotoRecipe_WithSameIds()
        {
            // Arrange
            var request = new CreatePhotoRecipeRequest
            {
                PhotoId = _validPhotoId,
                RecipeId = _validRecipeId
            };
      
            var existingPhoto = new PhotoRecipe { PhotoId = _validPhotoId, RecipeId = _validRecipeId };

            _mockPhotoRecipeService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<PhotoRecipe, bool>>>(), It.IsAny<Expression<Func<PhotoRecipe, object>>>()))
                .Returns((Expression<Func<PhotoRecipe, bool>> predicate, Expression<Func<PhotoRecipe, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingPhoto) ? existingPhoto : null);
                });

            // Act
            var result = await _controller.AddPhotoRecipe(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }


        [Test]
        public async Task AddPhotoRecipe_InvalidPhotoId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreatePhotoRecipeRequest
            {
                PhotoId = -1, 
                RecipeId = _validRecipeId,
                
            };

            // Act
            var result = await _controller.AddPhotoRecipe(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        
        }

        [Test]
        public async Task AddPhotoRecipe_InvalidRecipeId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreatePhotoRecipeRequest
            {
                PhotoId = _validPhotoId,
                RecipeId = -1, 
                
            };

            // Act
            var result = await _controller.AddPhotoRecipe(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
         
          
        }



    }

}
