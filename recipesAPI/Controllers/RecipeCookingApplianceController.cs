using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("RecipeCookingAppliance")]
    public class RecipeCookingApplianceController : ControllerBase
    {
        private readonly IEntityService<RecipeCookingAppliance> _recipeCookingApplianceService;
        private readonly IValidator<CreateRecipeCookingApplianceRequest> _validator;

        public RecipeCookingApplianceController(IEntityService<RecipeCookingAppliance> recipeCookingApplianceService, IValidator<CreateRecipeCookingApplianceRequest> validator)
        {
            _recipeCookingApplianceService = recipeCookingApplianceService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeCookingApplianceResponse>> AddRecipeCookingAppliance(CreateRecipeCookingApplianceRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var recipeCookingAppliance = new RecipeCookingAppliance
            {
                RecipeId = request.RecipeId,
                CookingApplianceId = request.CookingApplianceId,
                CreatedOn = DateTime.UtcNow
            };

            await _recipeCookingApplianceService.AddAsync(recipeCookingAppliance);

            var response = new RecipeCookingApplianceResponse
            {
                RecipeCookingApplianceId = recipeCookingAppliance.RecipeCookingApplianceId,
                RecipeId = recipeCookingAppliance.RecipeId,
                CookingApplianceId = recipeCookingAppliance.CookingApplianceId,
                CreatedOn = recipeCookingAppliance.CreatedOn
            };

            return Ok(response);
        }
    }

}
