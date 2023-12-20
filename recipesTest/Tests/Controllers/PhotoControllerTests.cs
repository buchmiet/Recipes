using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using recipesAPI.Controllers;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;
using Microsoft.AspNetCore.Http.HttpResults;

namespace recipesTest.Tests.Controllers
{
    [TestFixture]
    public class PhotoControllerTests
    {
        private PhotoController _controller;
        private CreatePhotoRequestValidator _validator;
        private Mock<IEntityService<Photo>> _photoService;


        [SetUp]
        public void Setup()
        {
            _validator = new CreatePhotoRequestValidator();
            _photoService = new Mock<IEntityService<Photo>>(); 

            _controller = new PhotoController(_photoService.Object, _validator );
        }

        [Test]
        public async Task AddPhoto_ValidData_ReturnsOk()
        {
            // Arrange
            var request = new CreatePhotoRequest
            {
                Address = "TestPhoto",
            };
            var photo = new Photo
            {
                Address = request.Address,
                CreatedOn = DateTime.UtcNow
            };
            _photoService.Setup(s => s.AddAsync(It.IsAny<Photo>(), It.IsAny<bool>()))
                         .Returns(Task.CompletedTask)
                        .Callback<Photo, bool>((a, b) => photo.PhotoId = 1);

            // Act
            var result = await _controller.AddPhoto(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var response = okResult.Value as PhotoResponse;
            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                // Dodaj asercje sprawdzające odpowiedź zgodnie z oczekiwaniami.
            });
        }

        [Test]
        public async Task AddPhoto_NullTagName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreatePhotoRequest
            {
                Address = null
            };

            // Act
            var result = await _controller.AddPhoto(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task AddPhoto_EmptyTagName_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreatePhotoRequest
            {
                Address = string.Empty
            };

            // Act
            var result = await _controller.AddPhoto(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }

}
