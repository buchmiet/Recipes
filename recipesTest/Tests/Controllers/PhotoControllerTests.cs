using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using recipesAPI.Controllers;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;

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
            _photoService = new Mock<IEntityService<Photo>>();
            _validator = new CreatePhotoRequestValidator(_photoService.Object);
       

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

        [Test]
        public async Task AddPhoto_WithExistingAddress_ReturnsBadRequest()
        {
            // Arrange
            var existingAddress = "www.internet.com/photo.jpg";
            var request = new CreatePhotoRequest { Address = existingAddress };
            var existingPhoto = new Photo { PhotoId = 1, Address = existingAddress };

            _photoService.Setup(s => s.GetOneAsync(It.IsAny<Expression<Func<Photo, bool>>>(), It.IsAny<Expression<Func<Photo, object>>>()))
                .Returns((Expression<Func<Photo, bool>> predicate, Expression<Func<Photo, object>> orderByDescending) =>
                {
                    var compiledPredicate = predicate.Compile();
                    return Task.FromResult(compiledPredicate(existingPhoto) ? existingPhoto : null);
                });

            // Act
            var result = await _controller.AddPhoto(request);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }


    }

}
