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
    public class IngredientControllerTests
    {
        private IngredientController _controller;
        private Mock<IEntityService<Ingredient>> _mockIngredientService;
        private IValidator<CreateIngredientRequest> _validator;
        private Mock<IEntityService<IngredientType>> _ingredientTypeService;
        private Mock<IEntityService<IngredientAmountType>> _ingredientAmountTypeService;
        int _validIngredientTypeId = 123;
        int _validIngredientAmountTypeId = 456;

        [SetUp]
        public void Setup()
        {
            _ingredientTypeService = new Mock<IEntityService<IngredientType>>();
            _ingredientAmountTypeService = new Mock<IEntityService<IngredientAmountType>>();
            var ingredientType = new IngredientType { IngredientTypeId = _validIngredientTypeId };
            var ingredientAmountType = new IngredientAmountType { IngredientAmountTypeId = _validIngredientAmountTypeId };

            _ingredientTypeService.Setup(s => s.GetByIdAsync(_validIngredientTypeId))
                             .ReturnsAsync(ingredientType);

            _ingredientAmountTypeService.Setup(s => s.GetByIdAsync(_validIngredientAmountTypeId))
                          .ReturnsAsync(ingredientAmountType);

            _mockIngredientService = new Mock<IEntityService<Ingredient>>();
            _validator = new CreateIngredientRequestValidator(_ingredientTypeService.Object, _mockIngredientService.Object, _ingredientAmountTypeService.Object); 

            _controller = new IngredientController(_mockIngredientService.Object, _validator);
        }

        [Test]
        public async Task AddIngredient_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateIngredientRequest
            {
                IngredientName = "TestIngredient",
                Type = _validIngredientTypeId,
                AmountType=_validIngredientAmountTypeId
            };
            var ingredient = new Ingredient
            {
                IngredientName = request.IngredientName,
                Type = request.Type,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };
            _mockIngredientService.Setup(s => s.AddAsync(It.IsAny<Ingredient>(), It.IsAny<bool>()))
                                  .Returns(Task.CompletedTask)
                                  .Callback<Ingredient, bool>((a, b) => ingredient.IngredientId = 1);

            // Act
            var result = await _controller.AddIngredient(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as IngredientResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.IngredientName, Is.EqualTo(request.IngredientName));
                Assert.That(response.Type, Is.EqualTo(request.Type));
              
            });
        }

        [Test]
        public async Task AddIngredient_NullIngredientName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateIngredientRequest
            {
                IngredientName = null,
                Type = _validIngredientTypeId,
                AmountType = _validIngredientAmountTypeId
            };

            // Act
            var result = await _controller.AddIngredient(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredient_EmptyIngredientName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateIngredientRequest
            {
                IngredientName = string.Empty,
                Type = _validIngredientTypeId,
                AmountType=_validIngredientAmountTypeId
            };

            // Act
            var result = await _controller.AddIngredient(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        public async Task AddIngredient_InvalidTypeId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateIngredientRequest
            {

                IngredientName = "ValidName",
                Type = 1
            };

            var controller = new IngredientController(_mockIngredientService.Object, _validator);

            // Act
            var result = await controller.AddIngredient(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        public async Task AddIngredient_InvalidAmountTypeId_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateIngredientRequest
            {

                IngredientName = "ValidName",
                Type = _validIngredientTypeId,
                AmountType = -1
            };

            var controller = new IngredientController(_mockIngredientService.Object, _validator);

            // Act
            var result = await controller.AddIngredient(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredient_WithExistingName_ReturnsBadRequest()
        {
            // Arrange
            var existingIngredientName = "salt";
            var request = new CreateIngredientRequest { IngredientName = existingIngredientName };
            var existingIngredient = new Ingredient { IngredientId = 1, IngredientName = existingIngredientName };

            _mockIngredientService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<Ingredient, bool>>>(), It.IsAny<Expression<Func<Ingredient, object>>>()))
                .Returns((Expression<Func<Ingredient, bool>> predicate, Expression<Func<Ingredient, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingIngredient) ? existingIngredient : null);
                });

            // Act
            var result = await _controller.AddIngredient(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

    }
}
