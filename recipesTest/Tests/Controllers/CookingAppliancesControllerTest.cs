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
    public class CookingAppliancesControllerTests
    {
        private CookingAppliancesController _controller;
        private Mock<IEntityService<CookingAppliance>> _mockCookingApplianceService;
        private IValidator<CreateCookingApplianceRequest> _validator;

        [SetUp]
        public void Setup()
        {
            _mockCookingApplianceService = new Mock<IEntityService<CookingAppliance>>();
            _validator = new CreateCookingApplianceRequestValidator(_mockCookingApplianceService.Object); 
            _controller = new CookingAppliancesController(_mockCookingApplianceService.Object, _validator);
        }

        [Test]
        public async Task AddCookingAppliance_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreateCookingApplianceRequest
            {
                Name = "TestAppliance",
            };
            var cookingAppliance = new CookingAppliance
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };
            _mockCookingApplianceService.Setup(s => s.AddAsync(It.IsAny<CookingAppliance>(), It.IsAny<bool>()))
                                        .Returns(Task.CompletedTask)
                                        .Callback<CookingAppliance, bool>((a, b) => cookingAppliance.CookingApplianceId = 1);

            // Act
            var result = await _controller.AddCookingAppliance(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as CookingApplianceResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Name, Is.EqualTo(request.Name));
                
            });
        }

        [Test]
        public async Task AddCookingAppliance_NullName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateCookingApplianceRequest
            {
                Name = null
            };

            // Act
            var result = await _controller.AddCookingAppliance(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddCookingAppliance_EmptyName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateCookingApplianceRequest
            {
                Name = string.Empty
            };

            // Act
            var result = await _controller.AddCookingAppliance(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddCookingAppliance_WithExistingName_ReturnsBadRequest()
        {
            // Arrange
            var existingName = "microwave";
            var request = new CreateCookingApplianceRequest { Name = existingName };
            var existingCookingAppliance = new CookingAppliance { CookingApplianceId = 1, Name = existingName };

            _mockCookingApplianceService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<CookingAppliance, bool>>>(), It.IsAny<Expression<Func<CookingAppliance, object>>>()))
                .Returns((Expression<Func<CookingAppliance, bool>> predicate, Expression<Func<CookingAppliance, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingCookingAppliance) ? existingCookingAppliance : null);
                });

            // Act
            var result = await _controller.AddCookingAppliance(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

    }
}