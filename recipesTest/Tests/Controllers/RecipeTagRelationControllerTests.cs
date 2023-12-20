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
    public class RecipeTagRelationControllerTests
    {
        private RecipeTagRelationController _controller;
        private Mock<IEntityService<RecipeTagRelation>> _mockRecipeTagRelationService;
        private Mock<IEntityService<Recipe>> _mockRecipeService;
        private Mock<IEntityService<Tag>> _mockTagService;
        private IValidator<CreateRecipeTagRelationRequest> _validator;
        private int _validRecipeId = 70;
        private int _validTagId = 80;

        [SetUp]
        public void Setup()
        {
            _mockRecipeService = new Mock<IEntityService<Recipe>>();
            _mockTagService = new Mock<IEntityService<Tag>>();

            var recipe = new Recipe { RecipeId = _validRecipeId };
            var tag = new Tag { TagId = _validTagId };

            _mockRecipeService.Setup(s => s.GetByIdAsync(_validRecipeId))
                              .ReturnsAsync(recipe);
            _mockTagService.Setup(t => t.GetByIdAsync(_validTagId))
                           .ReturnsAsync(tag);

            _mockRecipeTagRelationService = new Mock<IEntityService<RecipeTagRelation>>();

            _validator = new CreateRecipeTagRelationRequestValidator(_mockRecipeService.Object,_mockTagService.Object, _mockRecipeTagRelationService.Object);

            _controller = new RecipeTagRelationController(_mockRecipeTagRelationService.Object, _validator);
        }

        [Test]
        public async Task AddRecipeTagRelation_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateRecipeTagRelationRequest
            {
                RecipeId = _validRecipeId,
                TagId = _validTagId
            };

            var recipeTagRelation = new RecipeTagRelation
            {
                RecipeId = request.RecipeId,
                TagId = request.TagId,
                CreatedOn = DateTime.UtcNow
            };

            _mockRecipeTagRelationService.Setup(s => s.AddAsync(It.IsAny<RecipeTagRelation>(), It.IsAny<bool>()))
                                         .Returns(Task.CompletedTask)
                                         .Callback<RecipeTagRelation, bool>((rtr, b) => rtr.RecipeTagRelationId = 1);

            // Act
            var result = await _controller.AddRecipeTagRelation(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as RecipeTagRelationResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.RecipeId, Is.EqualTo(request.RecipeId));
                Assert.That(response.TagId, Is.EqualTo(request.TagId));
            });
        }

        [Test]
public async Task AddRecipeTagRelation_InvalidRecipeId_ReturnsBadRequest()
{
    // Arrange
    var request = new CreateRecipeTagRelationRequest
    {
        RecipeId = -1, // nieistniejący RecipeId
        TagId = _validTagId
    };

    _mockRecipeService.Setup(s => s.GetByIdAsync(-1))
                      .ReturnsAsync((Recipe)null); // Brak Recipe dla nieistniejącego ID

    // Act
    var result = await _controller.AddRecipeTagRelation(request);

    // Assert
    Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
}

[Test]
public async Task AddRecipeTagRelation_InvalidTagId_ReturnsBadRequest()
{
    // Arrange
    var request = new CreateRecipeTagRelationRequest
    {
        RecipeId = _validRecipeId,
        TagId = -1 // nieistniejący TagId
    };

    _mockTagService.Setup(t => t.GetByIdAsync(-1))
                   .ReturnsAsync((Tag)null); // Brak Tag dla nieistniejącego ID

    // Act
    var result = await _controller.AddRecipeTagRelation(request);

    // Assert
    Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
}



        [Test]
        public async Task AddRecipeTagRelation_WithSameIds()
        {
            // Arrange
            var request = new CreateRecipeTagRelationRequest
            {
                RecipeId = _validRecipeId,
                TagId = _validTagId
            };

            var existingRecipeTagRelation = new RecipeTagRelation
            {
                RecipeTagRelationId = 102, // Przykładowe ID
                RecipeId = _validRecipeId,
                TagId = _validTagId
            };

            _mockRecipeTagRelationService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<RecipeTagRelation, bool>>>(), It.IsAny<Expression<Func<RecipeTagRelation, object>>>()))
                .Returns((Expression<Func<RecipeTagRelation, bool>> predicate, Expression<Func<RecipeTagRelation, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingRecipeTagRelation) ? existingRecipeTagRelation : null);
                });

            // Act
            var result = await _controller.AddRecipeTagRelation(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }


    }



}
