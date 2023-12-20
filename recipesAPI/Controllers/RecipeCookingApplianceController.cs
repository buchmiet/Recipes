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

        public RecipeCookingApplianceController(IEntityService<RecipeCookingAppliance> recipeCookingApplianceService)
        {
            _recipeCookingApplianceService = recipeCookingApplianceService;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeCookingApplianceResponse>> AddRecipeCookingAppliance(CreateRecipeCookingApplianceRequest request)
        {
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
