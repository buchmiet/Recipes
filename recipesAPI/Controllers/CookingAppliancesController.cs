using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.DataAccess;
using static recipesCommon.DataAccess.RecipesDbContext;
using recipesCommon.Model.Request;
using Microsoft.EntityFrameworkCore;
using recipesCommon.Model.Response;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("CookingAppliances")]
    public class CookingAppliancesController : ControllerBase
    {
        private readonly IEntityService<CookingAppliance> _cookingApplianceService;

        public CookingAppliancesController(IEntityService<CookingAppliance> cookingApplianceService)
        {
            _cookingApplianceService = cookingApplianceService;
        }

        [HttpPost]
        public async Task<ActionResult<CookingApplianceResponse>> AddCookingAppliance(CreateCookingApplianceRequest request)
        {
            var cookingAppliance = new CookingAppliance
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };

            await _cookingApplianceService.AddAsync(cookingAppliance);

            var response = new CookingApplianceResponse
            {
                CookingApplianceId = cookingAppliance.CookingApplianceId,
                Name = cookingAppliance.Name,
                CreatedOn = cookingAppliance.CreatedOn,
                LastModifiedOn = cookingAppliance.LastModifiedOn
            };

            return Ok(response);
        }

    }

}
