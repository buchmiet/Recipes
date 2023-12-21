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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesTest.Tests.Controllers
{
    [TestFixture]
    public class RecipeCookingApplianceControllerTests
    {
        private RecipeCookingApplianceController _controller;
        private Mock<IEntityService<RecipeCookingAppliance>> _mockRecipeCookingApplianceService;
        private Mock<IEntityService<Recipe>> _mockRecipeService;
        private Mock<IEntityService<CookingAppliance>> _mockCookingApplianceService;
        private IValidator<CreateRecipeCookingApplianceRequest> _validator;
        private int _validRecipeId = 110;
        private int _validCookingApplianceId = 120;

        [SetUp]
        public void Setup()
        {
            _mockRecipeService = new Mock<IEntityService<Recipe>>();
            _mockCookingApplianceService = new Mock<IEntityService<CookingAppliance>>();

            var recipe = new Recipe { RecipeId = _validRecipeId };
            var cookingAppliance = new CookingAppliance { CookingApplianceId = _validCookingApplianceId };

            _mockRecipeService.Setup(r => r.GetByIdAsync(_validRecipeId))
                              .ReturnsAsync(recipe);
            _mockCookingApplianceService.Setup(c => c.GetByIdAsync(_validCookingApplianceId))
                                        .ReturnsAsync(cookingAppliance);

            _mockRecipeCookingApplianceService = new Mock<IEntityService<RecipeCookingAppliance>>();

            _validator = new CreateRecipeCookingApplianceRequestValidator(_mockRecipeService.Object, _mockCookingApplianceService.Object, _mockRecipeCookingApplianceService.Object);

            _controller = new RecipeCookingApplianceController(_mockRecipeCookingApplianceService.Object, _validator);
        }

        [Test]
        public async Task AddRecipeCookingAppliance_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateRecipeCookingApplianceRequest
            {
                RecipeId = _validRecipeId,
                CookingApplianceId = _validCookingApplianceId
            };

            var recipeCookingAppliance = new RecipeCookingAppliance
            {
                RecipeId = request.RecipeId,
                CookingApplianceId = request.CookingApplianceId
            };

            _mockRecipeCookingApplianceService.Setup(s => s.AddAsync(It.IsAny<RecipeCookingAppliance>(), It.IsAny<bool>()))
                                              .Returns(Task.CompletedTask)
                                              .Callback<RecipeCookingAppliance, bool>((rca, b) => rca.RecipeCookingApplianceId = 1);

            // Act
            var result = await _controller.AddRecipeCookingAppliance(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as RecipeCookingApplianceResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.RecipeId, Is.EqualTo(request.RecipeId));
                Assert.That(response.CookingApplianceId, Is.EqualTo(request.CookingApplianceId));
            });
        }
        [Test]
        public async Task AddRecipeCookingAppliance_InvalidRecipeId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeCookingApplianceRequest
            {
                RecipeId = -1, // nieprawidłowy RecipeId
                CookingApplianceId = _validCookingApplianceId
            };

            _mockRecipeService.Setup(r => r.GetByIdAsync(-1))
                              .ReturnsAsync((Recipe)null); // Brak Recipe dla nieistniejącego ID

            // Act
            var result = await _controller.AddRecipeCookingAppliance(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipeCookingAppliance_InvalidCookingApplianceId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateRecipeCookingApplianceRequest
            {
                RecipeId = _validRecipeId,
                CookingApplianceId = -1 // nieprawidłowy CookingApplianceId
            };

            _mockCookingApplianceService.Setup(c => c.GetByIdAsync(-1))
                                        .ReturnsAsync((CookingAppliance)null); // Brak CookingAppliance dla nieistniejącego ID

            // Act
            var result = await _controller.AddRecipeCookingAppliance(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddRecipeCookingAppliance_WithSameIds()
        {
            // Arrange
            var request = new CreateRecipeCookingApplianceRequest
            {
                RecipeId = _validRecipeId,
                CookingApplianceId = _validCookingApplianceId
            };

            var existingRecipeCookingAppliance = new RecipeCookingAppliance
            {
                RecipeCookingApplianceId = 100, // Przykładowe ID
                RecipeId = _validRecipeId,
                CookingApplianceId = _validCookingApplianceId
            };

            _mockRecipeCookingApplianceService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<RecipeCookingAppliance, bool>>>(), It.IsAny<Expression<Func<RecipeCookingAppliance, object>>>()))
                .Returns((Expression<Func<RecipeCookingAppliance, bool>> predicate, Expression<Func<RecipeCookingAppliance, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingRecipeCookingAppliance) ? existingRecipeCookingAppliance : null);
                });

            // Act
            var result = await _controller.AddRecipeCookingAppliance(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }


    }

}
