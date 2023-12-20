using FluentValidation;
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
        private readonly IValidator<CreateIngredientAmountTypeRequest> _validator;

        public IngredientAmountTypeController(IEntityService<IngredientAmountType> ingredientAmountTypeService, IValidator<CreateIngredientAmountTypeRequest> validator)
        {
            _ingredientAmountTypeService = ingredientAmountTypeService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<IngredientAmountTypeResponse>> AddIngredientAmountType(CreateIngredientAmountTypeRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
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
