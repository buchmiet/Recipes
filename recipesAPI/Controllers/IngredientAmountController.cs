using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("IngredientAmount")]
    public class IngredientAmountController : ControllerBase
    {
        private readonly IEntityService<IngredientAmount> _ingredientAmountService;

        public IngredientAmountController(IEntityService<IngredientAmount> ingredientAmountService)
        {
            _ingredientAmountService = ingredientAmountService;
        }

        [HttpPost]
        public async Task<ActionResult<IngredientAmountResponse>> AddIngredientAmount(CreateIngredientAmountRequest request)
        {
            var ingredientAmount = new IngredientAmount
            {
                IngredientAmountTypeId = request.IngredientAmountTypeId,
                IngredientId = request.IngredientId,
                Amount = request.Amount,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };

            await _ingredientAmountService.AddAsync(ingredientAmount);

            var response = new IngredientAmountResponse
            {
                IngredientAmountId = ingredientAmount.IngredientAmountId,
                IngredientAmountTypeId = ingredientAmount.IngredientAmountTypeId,
                IngredientId = ingredientAmount.IngredientId,
                Amount = ingredientAmount.Amount,
                CreatedOn = ingredientAmount.CreatedOn,
                LastModifiedOn = ingredientAmount.LastModifiedOn
            };

            return Ok(response);
        }
    }

}
