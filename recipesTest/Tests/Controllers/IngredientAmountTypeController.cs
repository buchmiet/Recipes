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
    public class IngredientAmountTypeControllerTests
    {
        private IngredientAmountTypeController _controller;
        private Mock<IEntityService<IngredientAmountType>> _mockIngredientAmountTypeService;
        private IValidator<CreateIngredientAmountTypeRequest> _validator;

        [SetUp]
        public void Setup()
        {
            _mockIngredientAmountTypeService = new Mock<IEntityService<IngredientAmountType>>();
            _validator = new CreateIngredientAmountTypeRequestValidator(_mockIngredientAmountTypeService.Object); 
            _controller = new IngredientAmountTypeController(_mockIngredientAmountTypeService.Object, _validator);
        }

        [Test]
        public async Task AddIngredientAmountType_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateIngredientAmountTypeRequest
            {
                UnitName = "TestUnit",
            };
            var ingredientAmountType = new IngredientAmountType
            {
                UnitName = request.UnitName
            };
            _mockIngredientAmountTypeService.Setup(s => s.AddAsync(It.IsAny<IngredientAmountType>(), It.IsAny<bool>()))
                                            .Returns(Task.CompletedTask)
                                            .Callback<IngredientAmountType, bool>((a, b) => ingredientAmountType.IngredientAmountTypeId = 1);

            // Act
            var result = await _controller.AddIngredientAmountType(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as IngredientAmountTypeResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.UnitName, Is.EqualTo(request.UnitName));                
            });
        }

        [Test]
        public async Task AddIngredientAmountType_NullUnitName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateIngredientAmountTypeRequest
            {
                UnitName = null
            };

            // Act
            var result = await _controller.AddIngredientAmountType(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredientAmountType_EmptyUnitName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateIngredientAmountTypeRequest
            {
                UnitName = string.Empty
            };

            // Act
            var result = await _controller.AddIngredientAmountType(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredientAmountType_WithExistingUnitName_ReturnsBadRequest()
        {
            // Arrange
            var existingUnitName = "Liter";
            var request = new CreateIngredientAmountTypeRequest { UnitName = existingUnitName };
            var existingIngredientAmountType = new IngredientAmountType { IngredientAmountTypeId = 1, UnitName = existingUnitName };

            _mockIngredientAmountTypeService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<IngredientAmountType, bool>>>(), It.IsAny<Expression<Func<IngredientAmountType, object>>>()))
                .Returns((Expression<Func<IngredientAmountType, bool>> predicate, Expression<Func<IngredientAmountType, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingIngredientAmountType) ? existingIngredientAmountType : null);
                });

            // Act
            var result = await _controller.AddIngredientAmountType(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }


    }
}
