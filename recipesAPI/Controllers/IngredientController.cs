using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Ingredient")]
    public class IngredientController : ControllerBase
    {
        private readonly IEntityService<Ingredient> _ingredientService;

        public IngredientController(IEntityService<Ingredient> ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpPost]
        public async Task<ActionResult<IngredientResponse>> AddIngredient(CreateIngredientRequest request)
        {
            var ingredient = new Ingredient
            {
                IngredientName = request.IngredientName,
                Type = request.Type,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };

            await _ingredientService.AddAsync(ingredient);

            var response = new IngredientResponse
            {
                IngredientId = ingredient.IngredientId,
                IngredientName = ingredient.IngredientName,
                Type = ingredient.Type,
                CreatedOn = ingredient.CreatedOn,
                LastModifiedOn = ingredient.LastModifiedOn
            };

            return Ok(response);
        }
    }

}
