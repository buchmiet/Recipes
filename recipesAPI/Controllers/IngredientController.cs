using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Ingredient")]
    public class IngredientController : ControllerBase
    {
        private readonly IEntityService<Ingredient> _ingredientService;
        private readonly IValidator<CreateIngredientRequest> _validator;

        public IngredientController(IEntityService<Ingredient> ingredientService, IValidator<CreateIngredientRequest> validator)
        {
            _ingredientService = ingredientService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<IngredientResponse>> AddIngredient(CreateIngredientRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
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
