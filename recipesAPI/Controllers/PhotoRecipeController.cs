using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("PhotoRecipe")]
    public class PhotoRecipeController : ControllerBase
    {
        private readonly IEntityService<PhotoRecipe> _photoRecipeService;
        private readonly IValidator<CreatePhotoRecipeRequest> _validator;

        public PhotoRecipeController(IEntityService<PhotoRecipe> photoRecipeService, IValidator<CreatePhotoRecipeRequest> validator)
        {
            _photoRecipeService = photoRecipeService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<PhotoRecipeResponse>> AddPhotoRecipe(CreatePhotoRecipeRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var photoRecipe = new PhotoRecipe
            {
                PhotoId = request.PhotoId,
                RecipeId = request.RecipeId,             
                CreatedOn = DateTime.UtcNow
            };
            
            var pos=(await _photoRecipeService.GetAllAsync(p=>p.RecipeId == request.RecipeId));
            if (pos!=null)
            {
                photoRecipe.Position= pos.Count();
            }

            await _photoRecipeService.AddAsync(photoRecipe);

            var response = new PhotoRecipeResponse
            {
                PhotoRecipeId = photoRecipe.PhotoRecipeId,
                PhotoId = photoRecipe.PhotoId,
                RecipeId = photoRecipe.RecipeId,
                Position = photoRecipe.Position,
                CreatedOn = photoRecipe.CreatedOn
            };

            return Ok(response);
        }
    }

}
