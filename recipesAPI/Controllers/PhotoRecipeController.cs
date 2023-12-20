using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("PhotoRecipe")]
    public class PhotoRecipeController : ControllerBase
    {
        private readonly IEntityService<PhotoRecipe> _photoRecipeService;

        public PhotoRecipeController(IEntityService<PhotoRecipe> photoRecipeService)
        {
            _photoRecipeService = photoRecipeService;
        }

        [HttpPost]
        public async Task<ActionResult<PhotoRecipeResponse>> AddPhotoRecipe(CreatePhotoRecipeRequest request)
        {
            var photoRecipe = new PhotoRecipe
            {
                PhotoId = request.PhotoId,
                RecipeId = request.RecipeId,
                Position = request.Position,
                CreatedOn = DateTime.UtcNow
            };

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
