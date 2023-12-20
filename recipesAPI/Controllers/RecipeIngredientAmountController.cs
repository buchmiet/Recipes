using FluentValidation;
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
        private readonly IEntityService<RecipeIngredient> _recipeIngredientAmountService;
        private readonly IValidator<CreateRecipeIngredientAmountRequest> _validator;

        public RecipeIngredientAmountController(IEntityService<RecipeIngredient> recipeIngredientAmountService, IValidator<CreateRecipeIngredientAmountRequest> validator)
        {
            _recipeIngredientAmountService = recipeIngredientAmountService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeIngredientAmountResponse>> AddRecipeIngredientAmount(CreateRecipeIngredientAmountRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var recipeIngredientAmount = new RecipeIngredient
            {
                RecipeId = request.RecipeId,
                IngredientId = request.IngredientId,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow,
                IngredientAmount= request.IngredientAmount
            };

            await _recipeIngredientAmountService.AddAsync(recipeIngredientAmount);

            var response = new RecipeIngredientAmountResponse
            {
                RecipeIngredientAmountId = recipeIngredientAmount.RecipeIngredientId,
                RecipeId = recipeIngredientAmount.RecipeId,
                IngredientId = recipeIngredientAmount.IngredientId,
                Amount=recipeIngredientAmount.IngredientAmount,
                CreatedOn = recipeIngredientAmount.CreatedOn,
                LastModifiedOn = recipeIngredientAmount.LastModifiedOn
            };

            return Ok(response);
        }
    }

}
