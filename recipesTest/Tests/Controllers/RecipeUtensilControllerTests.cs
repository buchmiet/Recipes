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
    public class RecipeUtensilControllerTests
    {
        private RecipeUtensilController _controller;
        private Mock<IEntityService<RecipeUtensil>> _mockRecipeUtensilService;
        private Mock<IEntityService<Recipe>> _mockRecipeService;
        private Mock<IEntityService<Utensil>> _mockUtensilService;
        private IValidator<CreateRecipeUtensilRequest> _validator;
        private int _validRecipeId = 10;
        private int _validUtensilId = 20;

        [SetUp]
        public void Setup()
        {
            _mockRecipeService = new Mock<IEntityService<Recipe>>();
            _mockUtensilService = new Mock<IEntityService<Utensil>>();

            var recipe = new Recipe { RecipeId = _validRecipeId };
            var utensil = new Utensil { UtensilId = _validUtensilId };

            _mockRecipeService.Setup(s => s.GetByIdAsync(_validRecipeId))
                              .ReturnsAsync(recipe);
            _mockUtensilService.Setup(u => u.GetByIdAsync(_validUtensilId))
                               .ReturnsAsync(utensil);

            _mockRecipeUtensilService = new Mock<IEntityService<RecipeUtensil>>();

            _validator = new CreateRecipeUtensilRequestValidator(_mockRecipeService.Object, _mockUtensilService.Object, _mockRecipeUtensilService.Object);

            _controller = new RecipeUtensilController(_mockRecipeUtensilService.Object, _validator);
        }

        [Test]
        public async Task AddRecipeUtensil_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateRecipeUtensilRequest
            {
                RecipeId = _validRecipeId,
                UtensilId = _validUtensilId
            };

            var recipeUtensil = new RecipeUtensil
            {
                RecipeId = request.RecipeId,
                UtensilId = request.UtensilId,
                CreatedOn = DateTime.UtcNow
            };

            _mockRecipeUtensilService.Setup(s => s.AddAsync(It.IsAny<RecipeUtensil>(), It.IsAny<bool>()))
                                     .Returns(Task.CompletedTask)
                                     .Callback<RecipeUtensil, bool>((ru, b) => ru.RecipeUtensilId = 1);

            // Act
            var result = await _controller.AddRecipeUtensil(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as RecipeUtensilResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.RecipeId, Is.EqualTo(request.RecipeId));
                Assert.That(response.UtensilId, Is.EqualTo(request.UtensilId));
            });
        }

        [Test]
        public async Task AddRecipeUtensil_InvalidRecipeId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeUtensilRequest
            {
                RecipeId = -1, // nieprawidłowy RecipeId
                UtensilId = _validUtensilId
            };

            // Act
            var result = await _controller.AddRecipeUtensil(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
          
        }

        [Test]
        public async Task AddRecipeUtensil_InvalidUtensilId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeUtensilRequest
            {
                RecipeId = _validRecipeId,
                UtensilId = -1 // nieprawidłowy UtensilId
            };

            // Act
            var result = await _controller.AddRecipeUtensil(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
         
          
        }

        [Test]
        public async Task AddRecipeUtensil_WithSameIds()
        {
            // Arrange
            var request = new CreateRecipeUtensilRequest
            {
                RecipeId = _validRecipeId,
                UtensilId = _validUtensilId
            };

            var existingRecipeUtensil = new RecipeUtensil
            {
                RecipeUtensilId = 103, // Przykładowe ID
                RecipeId = _validRecipeId,
                UtensilId = _validUtensilId
            };

            _mockRecipeUtensilService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<RecipeUtensil, bool>>>(), It.IsAny<Expression<Func<RecipeUtensil, object>>>()))
                .Returns((Expression<Func<RecipeUtensil, bool>> predicate, Expression<Func<RecipeUtensil, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingRecipeUtensil) ? existingRecipeUtensil : null);
                });

            // Act
            var result = await _controller.AddRecipeUtensil(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }


    }

}
