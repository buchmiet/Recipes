using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesAPI.Model.Request;
using recipesAPI.Services;
using recipesCommon.Interfaces;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Recipe")]
    public class RecipeController : ControllerBase
    {
        private readonly IEntityService<Recipe> _recipeService;
        private readonly IValidator<CreateRecipeRequest> _createValidator;
        private readonly ISearchService _searchService;

        public RecipeController(ISearchService searchService, IEntityService<Recipe> recipeService, IValidator<CreateRecipeRequest> createValidator)
        {
            _recipeService = recipeService;
            _createValidator = createValidator;
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeResponse>> AddRecipe(CreateRecipeRequest request)
        {
            var validationResult = await _createValidator.ValidateAsync(request);
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
            await _searchService.UpdateSearchtermForRecipe(recipe.RecipeId);

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

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeResponse>> GetRecipe(int id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            var response = MapToRecipeResponse(recipe);
            return Ok(response);
        }

     
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, UpdateRecipeRequest request)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            recipe.Title = request.Title;
            recipe.CookingTime = request.CookingTime;
          
            recipe.LastModifiedOn = DateTime.UtcNow;
            await _recipeService.UpdateAsync(recipe);
            await _searchService.UpdateSearchtermForRecipe(recipe.RecipeId);

            return NoContent();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            await _recipeService.DeleteAsync(recipe);

            return NoContent();
        }

        private RecipeResponse MapToRecipeResponse(Recipe recipe)
        {
            return new RecipeResponse
            {
                RecipeId = recipe.RecipeId,
                Title = recipe.Title,
                CookingTime = recipe.CookingTime,
                AuthorId = recipe.AuthorId,
                CreatedOn = recipe.CreatedOn,
                LastModifiedOn = recipe.LastModifiedOn
            };
        }
    }


}


