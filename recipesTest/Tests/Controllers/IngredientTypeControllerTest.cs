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
    public class IngredientTypeControllerTests
    {
        private IngredientTypeController _controller;
        private Mock<IEntityService<IngredientType>> _mockIngredientTypeService;
        private IValidator<CreateIngredientTypeRequest> _validator;

        [SetUp]
        public void Setup()
        {
            _mockIngredientTypeService = new Mock<IEntityService<IngredientType>>();
            _validator = new CreateIngredientTypeRequestValidator(_mockIngredientTypeService.Object); 

            _controller = new IngredientTypeController(_mockIngredientTypeService.Object, _validator);
        }

        [Test]
        public async Task AddIngredientType_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateIngredientTypeRequest
            {
                Name = "TestIngredientType",
            };
            var ingredientType = new IngredientType
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };
            _mockIngredientTypeService.Setup(s => s.AddAsync(It.IsAny<IngredientType>(), It.IsAny<bool>()))
                                      .Returns(Task.CompletedTask)
                                      .Callback<IngredientType, bool>((a, b) => ingredientType.IngredientTypeId = 1);

            // Act
            var result = await _controller.AddIngredientType(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as IngredientTypeResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Name, Is.EqualTo(request.Name));                
            });
        }

        [Test]
        public async Task AddIngredientType_NullName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateIngredientTypeRequest
            {
                Name = null
            };

            // Act
            var result = await _controller.AddIngredientType(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredientType_EmptyName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateIngredientTypeRequest
            {
                Name = string.Empty
            };

            // Act
            var result = await _controller.AddIngredientType(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddIngredientType_WithExistingName_ReturnsBadRequest()
        {
            // Arrange
            var existingName = "Fruit";
            var request = new CreateIngredientTypeRequest { Name = existingName };
            var existingIngredientType = new IngredientType { IngredientTypeId = 1, Name = existingName };

            _mockIngredientTypeService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<IngredientType, bool>>>(), It.IsAny<Expression<Func<IngredientType, object>>>()))
                .Returns((Expression<Func<IngredientType, bool>> predicate, Expression<Func<IngredientType, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingIngredientType) ? existingIngredientType : null);
                });

            // Act
            var result = await _controller.AddIngredientType(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }


    }
}
