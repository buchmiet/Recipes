using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("RecipeIngredientAmount")]
    public class RecipeIngredientAmountController : ControllerBase
    {
        private readonly IEntityService<RecipeIngredientAmount> _recipeIngredientAmountService;

        public RecipeIngredientAmountController(IEntityService<RecipeIngredientAmount> recipeIngredientAmountService)
        {
            _recipeIngredientAmountService = recipeIngredientAmountService;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeIngredientAmountResponse>> AddRecipeIngredientAmount(CreateRecipeIngredientAmountRequest request)
        {
            var recipeIngredientAmount = new RecipeIngredientAmount
            {
                RecipeId = request.RecipeId,
                IngredientAmountId = request.IngredientAmountId,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };

            await _recipeIngredientAmountService.AddAsync(recipeIngredientAmount);

            var response = new RecipeIngredientAmountResponse
            {
                RecipeIngredientAmountId = recipeIngredientAmount.RecipeIngredientAmountId,
                RecipeId = recipeIngredientAmount.RecipeId,
                IngredientAmountId = recipeIngredientAmount.IngredientAmountId,
                CreatedOn = recipeIngredientAmount.CreatedOn,
                LastModifiedOn = recipeIngredientAmount.LastModifiedOn
            };

            return Ok(response);
        }
    }

}
