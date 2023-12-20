using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("RecipeUtensil")]
    public class RecipeUtensilController : ControllerBase
    {
        private readonly IEntityService<RecipeUtensil> _recipeUtensilService;

        public RecipeUtensilController(IEntityService<RecipeUtensil> recipeUtensilService)
        {
            _recipeUtensilService = recipeUtensilService;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeUtensilResponse>> AddRecipeUtensil(CreateRecipeUtensilRequest request)
        {
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
