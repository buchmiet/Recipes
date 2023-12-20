using Microsoft.AspNetCore.Mvc;
using Moq;
using recipesAPI.Controllers;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesTest.Tests.Controllers
{

    public class CookingActionControllerTests
    {
        private CookingActionController _controller;
        private CreateCookingActionRequestValidator _validator;
        private Mock<IEntityService<CookingAction>> _cookingActionService;
        private Mock<IEntityService<Recipe>> _recipeService;
        int _validRecipeId = 123;

        [SetUp]
        public void Setup()
        {
            int validRecipeId = 123;
            var recipe = new Recipe { RecipeId = _validRecipeId }; 
            _recipeService = new Mock<IEntityService<Recipe>>();
            _recipeService.Setup(s => s.GetByIdAsync(validRecipeId))
                             .ReturnsAsync(recipe);
            _validator = new CreateCookingActionRequestValidator(_recipeService.Object);
            _cookingActionService = new Mock<IEntityService<CookingAction>>();
            _controller = new CookingActionController(_cookingActionService.Object, _validator);
        }

        [Test]
      
        public async Task AddCookingAction_ValidData_ReturnsOk()
        {
            // Arrange                   
            var request = new CreateCookingActionRequest
            {
                RecipeId = _validRecipeId,
                Name = "TestCookingAction",
            
            };

         
            var cookingAction = new CookingAction
            {
                RecipeId = request.RecipeId,
                Name = request.Name,            
                CreatedOn = DateTime.UtcNow
            };

            _cookingActionService.Setup(s => s.AddAsync(It.IsAny<CookingAction>(), It.IsAny<bool>()))
                                     .Returns(Task.CompletedTask)
                                     .Callback<CookingAction, bool>((a, b) => cookingAction.CookingActionId = 1); 

            // Act
            var result = await _controller.AddCookingAction(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200)); 

            var response = okResult.Value as CookingActionResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);           
                Assert.That(response.RecipeId, Is.EqualTo(request.RecipeId));
                Assert.That(response.Name, Is.EqualTo(request.Name));                
            });
        }


        [Test]
        public async Task AddCookingAction_EmptyName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateCookingActionRequest
            {
                RecipeId = _validRecipeId,
                Name = string.Empty,
            
            };

            var controller = new CookingActionController(_cookingActionService.Object,  _validator);
            // Act
            var result = await controller.AddCookingAction(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddCookingAction_NullName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateCookingActionRequest
            {
                RecipeId = _validRecipeId,
                Name = null,
             
            };

            var controller = new CookingActionController(_cookingActionService.Object,  _validator);

            // Act
            var result = await controller.AddCookingAction(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

     

        public async Task AddCookingAction_InvalidRecipeId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateCookingActionRequest
            {
                RecipeId = 1,
                Name = "ValidName",
               
            };

            var controller = new CookingActionController(_cookingActionService.Object, _validator);

            // Act
            var result = await controller.AddCookingAction(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]

        public async Task AddCookingAction_WhenNoExistingEntries_SetsPositionToZero()
        {
            // Arrange
            var request = new CreateCookingActionRequest
            {
                RecipeId = _validRecipeId,
                Name = "ValidName",
            };

            var emptyCookingActions = new List<CookingAction>().AsQueryable();
            _cookingActionService.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<CookingAction, bool>>>()))
                           .ReturnsAsync((Expression<Func<CookingAction, bool>> predicate) => emptyCookingActions.Where(predicate.Compile()).AsQueryable());


            // Act
            var result = await _controller.AddCookingAction(request);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as CookingActionResponse;
            Assert.That(response.Position, Is.EqualTo(0));
        }

        [Test]
        public async Task AddCookingAction_WhenOneExistingEntry_SetsPositionToOne()
        {
            // Arrange
            var request = new CreateCookingActionRequest
            {
                RecipeId = _validRecipeId,
                Name = "ValidName",
            };

            var existingCookingActions = new List<CookingAction>
    {
        new CookingAction { RecipeId = _validRecipeId, Position = 0 }
    }.AsQueryable();

            _cookingActionService.Setup(s => s.GetAllAsync(It.IsAny<Expression<Func<CookingAction, bool>>>()))
                           .ReturnsAsync((Expression<Func<CookingAction, bool>> predicate) => existingCookingActions.Where(predicate.Compile()).AsQueryable());


           

            // Act
            var result = await _controller.AddCookingAction(request);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as CookingActionResponse;
            Assert.That(response.Position, Is.EqualTo(1));
        }


    }
}
