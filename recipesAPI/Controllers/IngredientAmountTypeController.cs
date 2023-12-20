using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("IngredientAmountType")]
    public class IngredientAmountTypeController : ControllerBase
    {
        private readonly IEntityService<IngredientAmountType> _ingredientAmountTypeService;

        public IngredientAmountTypeController(IEntityService<IngredientAmountType> ingredientAmountTypeService)
        {
            _ingredientAmountTypeService = ingredientAmountTypeService;
        }

        [HttpPost]
        public async Task<ActionResult<IngredientAmountTypeResponse>> AddIngredientAmountType(CreateIngredientAmountTypeRequest request)
        {
            var ingredientAmountType = new IngredientAmountType
            {
                UnitName = request.UnitName
            };

            await _ingredientAmountTypeService.AddAsync(ingredientAmountType);

            var response = new IngredientAmountTypeResponse
            {
                IngredientAmountTypeId = ingredientAmountType.IngredientAmountTypeId,
                UnitName = ingredientAmountType.UnitName
            };

            return Ok(response);
        }
    }

}
