using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("IngredientType")]
    public class IngredientTypeController : ControllerBase
    {
        private readonly IEntityService<IngredientType> _ingredientTypeService;

        public IngredientTypeController(IEntityService<IngredientType> ingredientTypeService)
        {
            _ingredientTypeService = ingredientTypeService;
        }

        [HttpPost]
        public async Task<ActionResult<IngredientTypeResponse>> AddIngredientType(CreateIngredientTypeRequest request)
        {
            var ingredientType = new IngredientType
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };

            await _ingredientTypeService.AddAsync(ingredientType);

            var response = new IngredientTypeResponse
            {
                IngredientTypeId = ingredientType.IngredientTypeId,
                Name = ingredientType.Name,
                CreatedOn = ingredientType.CreatedOn,
                LastModifiedOn = ingredientType.LastModifiedOn
            };

            return Ok(response);
        }
    }

}
