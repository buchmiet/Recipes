using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("RecipeUtensil")]
    public class RecipeUtensilController : ControllerBase
    {
        private readonly IEntityService<RecipeUtensil> _recipeUtensilService;
        private readonly IValidator<CreateRecipeUtensilRequest> _validator;

        public RecipeUtensilController(IEntityService<RecipeUtensil> recipeUtensilService, IValidator<CreateRecipeUtensilRequest> validator)
        {
            _validator = validator;
            _recipeUtensilService = recipeUtensilService;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeUtensilResponse>> AddRecipeUtensil(CreateRecipeUtensilRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var recipeUtensil = new RecipeUtensil
            {
                RecipeId = request.RecipeId,
                UtensilId = request.UtensilId,
                CreatedOn = DateTime.UtcNow
            };

            await _recipeUtensilService.AddAsync(recipeUtensil);

            var response = new RecipeUtensilResponse
            {
                RecipeUtensilId = recipeUtensil.RecipeUtensilId,
                RecipeId = recipeUtensil.RecipeId,
                UtensilId = recipeUtensil.UtensilId,
                CreatedOn = recipeUtensil.CreatedOn
            };

            return Ok(response);
        }
    }

}
