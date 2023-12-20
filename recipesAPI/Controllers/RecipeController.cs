using FluentValidation;
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
        private readonly IValidator<CreateRecipeRequest> _validator;

        public RecipeController(IEntityService<Recipe> recipeService, IValidator<CreateRecipeRequest> validator)
        {
            _recipeService = recipeService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeResponse>> AddRecipe(CreateRecipeRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
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
