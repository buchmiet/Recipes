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
using System.Text;
using System.Threading.Tasks;
using static recipesCommon.DataAccess.RecipesDbContext;

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
            _validator = new CreateUtensilRequestValidator();

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

    }

}
