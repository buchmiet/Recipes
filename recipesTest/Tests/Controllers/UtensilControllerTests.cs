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
    public class UtensilControllerTests
    {
        private UtensilController _controller;
        private Mock<IEntityService<Utensil>> _mockUtensilService;
        private IValidator<CreateUtensilRequest> _validator;

        [SetUp]
        public void Setup()
        {
            _mockUtensilService = new Mock<IEntityService<Utensil>>();
            _validator = new CreateUtensilRequestValidator(_mockUtensilService.Object);

            _controller = new UtensilController(_mockUtensilService.Object, _validator);
        }

        [Test]
        public async Task AddUtensil_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateUtensilRequest
            {
                Name = "TestUtensil",
            };
            var utensil = new Utensil
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };
            _mockUtensilService.Setup(s => s.AddAsync(It.IsAny<Utensil>(), It.IsAny<bool>()))
                             .Returns(Task.CompletedTask)
                           .Callback<Utensil, bool>((a, b) => utensil.UtensilId = 1);

            // Act
            var result = await _controller.AddUtensil(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as UtensilResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(request.Name, Is.EqualTo(response.Name));
            });
        }

        [Test]
        public async Task AddTag_NullTagName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateUtensilRequest
            {
                Name = null
            };

            // Act
            var result = await _controller.AddUtensil(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddTag_EmptyTagName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateUtensilRequest
            {
                Name = string.Empty
            };

            // Act
            var result = await _controller.AddUtensil(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddUtensil_WithExistingName_ReturnsBadRequest()
        {
            // Arrange
            var existingName = "Spoon";
            var request = new CreateUtensilRequest { Name = existingName };
            var existingUtensil = new Utensil { UtensilId = 1, Name = existingName };

            _mockUtensilService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<Utensil, bool>>>(), It.IsAny<Expression<Func<Utensil, object>>>()))
                .Returns((Expression<Func<Utensil, bool>> predicate, Expression<Func<Utensil, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingUtensil) ? existingUtensil : null);
                });

            // Act
            var result = await _controller.AddUtensil(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }


    }

}
