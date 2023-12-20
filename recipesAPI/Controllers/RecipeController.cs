using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Recipe")]
    public class RecipeController : ControllerBase
    {
        private readonly IEntityService<Recipe> _recipeService;

        public RecipeController(IEntityService<Recipe> recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeResponse>> AddRecipe(CreateRecipeRequest request)
        {
            var recipe = new Recipe
            {
                Title = request.Title,
                CookingTime = request.CookingTime,
                AuthorId = request.AuthorId,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };

          

            await _recipeService.AddAsync(recipe);

            var response = new RecipeResponse
            {
                RecipeId = recipe.RecipeId,
                Title = recipe.Title,
                CookingTime = recipe.CookingTime,
                AuthorId = recipe.AuthorId,
                CreatedOn = recipe.CreatedOn,
                LastModifiedOn = recipe.LastModifiedOn
            };

            return Ok(response);
        }
    }

}
