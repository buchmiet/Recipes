using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Photo")]
    public class PhotoController : ControllerBase
    {
        private readonly IEntityService<Photo> _photoService;
        private readonly IValidator<CreatePhotoRequest> _validator;

        public PhotoController(IEntityService<Photo> photoService, IValidator<CreatePhotoRequest> validator)
        {
            _photoService = photoService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<PhotoResponse>> AddPhoto(CreatePhotoRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var photo = new Photo
            {
                Address = request.Address,
                CreatedOn = DateTime.UtcNow
            };

            await _photoService.AddAsync(photo);

            var response = new PhotoResponse
            {
                PhotoId = photo.PhotoId,
                Address = photo.Address,
                CreatedOn = photo.CreatedOn
            };

            return Ok(response);
        }
    }

}
