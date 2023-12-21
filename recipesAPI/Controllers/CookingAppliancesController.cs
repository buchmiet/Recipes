using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.DataAccess;
using static recipesCommon.DataAccess.RecipesDbContext;
using recipesCommon.Model.Request;
using Microsoft.EntityFrameworkCore;
using recipesCommon.Model.Response;
using FluentValidation;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("CookingAppliances")]
    public class CookingAppliancesController : ControllerBase
    {
        private readonly IEntityService<CookingAppliance> _cookingApplianceService;
        private readonly IValidator<CreateCookingApplianceRequest> _validator;

        public CookingAppliancesController(IEntityService<CookingAppliance> cookingApplianceService, IValidator<CreateCookingApplianceRequest> validator)
        {
            _cookingApplianceService = cookingApplianceService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<CookingApplianceResponse>> AddCookingAppliance(CreateCookingApplianceRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var cookingAppliance = new CookingAppliance
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow,
               
            };

            await _cookingApplianceService.AddAsync(cookingAppliance);

            var response = new CookingApplianceResponse
            {
                CookingApplianceId = cookingAppliance.CookingApplianceId,
                Name = cookingAppliance.Name,
                CreatedOn = cookingAppliance.CreatedOn,
              
            };

            return Ok(response);
        }

    }

}
