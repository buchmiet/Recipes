using FluentValidation;
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
    [TestFixture]
    public class RecipeIngredientAmountControllerTests
    {
        private RecipeIngredientAmountController _controller;
        private Mock<IEntityService<RecipeIngredient>> _mockRecipeIngredientAmountService;
        private Mock<IEntityService<Recipe>> _mockRecipeService;
        private Mock<IEntityService<Ingredient>> _mockIngredientService;
        private IValidator<CreateRecipeIngredientAmountRequest> _validator;
        private int _validRecipeId = 90;
        private int _validIngredientId = 100;

        [SetUp]
        public void Setup()
        {
            _mockRecipeService = new Mock<IEntityService<Recipe>>();
            _mockIngredientService = new Mock<IEntityService<Ingredient>>();

            var recipe = new Recipe { RecipeId = _validRecipeId };
            var ingredient = new Ingredient { IngredientId = _validIngredientId };

            _mockRecipeService.Setup(r => r.GetByIdAsync(_validRecipeId))
                              .ReturnsAsync(recipe);
            _mockIngredientService.Setup(i => i.GetByIdAsync(_validIngredientId))
                                  .ReturnsAsync(ingredient);

            _mockRecipeIngredientAmountService = new Mock<IEntityService<RecipeIngredient>>();

            _validator = new CreateRecipeIngredientAmountRequestValidator(_mockRecipeService.Object, _mockIngredientService.Object, _mockRecipeIngredientAmountService.Object);

            _controller = new RecipeIngredientAmountController(_mockRecipeIngredientAmountService.Object, _validator);
        }

        [Test]
        public async Task AddRecipeIngredientAmount_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateRecipeIngredientAmountRequest
            {
                RecipeId = _validRecipeId,
                IngredientId = _validIngredientId,
                IngredientAmount = 10.0f
            };

            var recipeIngredient = new RecipeIngredient
            {
                RecipeId = request.RecipeId,
                IngredientId = request.IngredientId,
                IngredientAmount = request.IngredientAmount
            };

            _mockRecipeIngredientAmountService.Setup(s => s.AddAsync(It.IsAny<RecipeIngredient>(), It.IsAny<bool>()))
                                              .Returns(Task.CompletedTask)
                                              .Callback<RecipeIngredient, bool>((ri, b) => ri.RecipeIngredientId = 1);

            // Act
            var result = await _controller.AddRecipeIngredientAmount(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as RecipeIngredientAmountResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.RecipeId, Is.EqualTo(request.RecipeId));
                Assert.That(response.IngredientId, Is.EqualTo(request.IngredientId));
                Assert.That(response.Amount, Is.EqualTo(request.IngredientAmount));
            });
        }

        [Test]
        public async Task AddRecipeIngredientAmount_InvalidRecipeId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeIngredientAmountRequest
            {
                RecipeId = -1, // nieprawidłowy RecipeId
                IngredientId = _validIngredientId,
                IngredientAmount = 10.0f
            };

            _mockRecipeService.Setup(r => r.GetByIdAsync(-1))
                              .ReturnsAsync((Recipe)null); // Brak Recipe dla nieistniejącego ID

            // Act
            var result = await _controller.AddRecipeIngredientAmount(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipeIngredientAmount_InvalidIngredientId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeIngredientAmountRequest
            {
                RecipeId = _validRecipeId,
                IngredientId = -1, // nieprawidłowy IngredientId
                IngredientAmount = 10.0f
            };

            _mockIngredientService.Setup(i => i.GetByIdAsync(-1))
                                  .ReturnsAsync((Ingredient)null); // Brak Ingredient dla nieistniejącego ID

            // Act
            var result = await _controller.AddRecipeIngredientAmount(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipeIngredientAmount_InvalidIngredientAmount_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeIngredientAmountRequest
            {
                RecipeId = _validRecipeId,
                IngredientId = _validIngredientId,
                IngredientAmount = -1.0f // nieprawidłowa ilość składnika
            };

            // Act
            var result = await _controller.AddRecipeIngredientAmount(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipeIngredient_WithSameIds()
        {
            // Arrange
            var request = new CreateRecipeIngredientAmountRequest
            {
                RecipeId = _validRecipeId,
                IngredientId = _validIngredientId
            };

            var existingRecipeIngredient = new RecipeIngredient
            {
                RecipeIngredientId = 101, // Przykładowe ID
                RecipeId = _validRecipeId,
                IngredientId = _validIngredientId
            };

            _mockRecipeIngredientAmountService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<RecipeIngredient, bool>>>(), It.IsAny<Expression<Func<RecipeIngredient, object>>>()))
                .Returns((Expression<Func<RecipeIngredient, bool>> predicate, Expression<Func<RecipeIngredient, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingRecipeIngredient) ? existingRecipeIngredient : null);
                });

            // Act
            var result = await _controller.AddRecipeIngredientAmount(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

    }

}
