using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("IngredientType")]
    public class IngredientTypeController : ControllerBase
    {
        private readonly IEntityService<IngredientType> _ingredientTypeService;
        private readonly IValidator<CreateIngredientTypeRequest> _validator;

        public IngredientTypeController(IEntityService<IngredientType> ingredientTypeService, IValidator<CreateIngredientTypeRequest> validator)
        {
            _ingredientTypeService = ingredientTypeService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<IngredientTypeResponse>> AddIngredientType(CreateIngredientTypeRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
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
