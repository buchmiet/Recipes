using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Utensil")]
    public class UtensilController : ControllerBase
    {
        private readonly IEntityService<Utensil> _utensilService;
        private readonly IValidator<CreateUtensilRequest> _validator;

        public UtensilController(IEntityService<Utensil> utensilService, IValidator<CreateUtensilRequest> validator)
        {
            _validator = validator;
            _utensilService = utensilService;
        }

        [HttpPost]
        public async Task<ActionResult<UtensilResponse>> AddUtensil(CreateUtensilRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var utensil = new Utensil
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };

            await _utensilService.AddAsync(utensil);

            var response = new UtensilResponse
            {
                UtensilId = utensil.UtensilId,
                Name = utensil.Name,
                CreatedOn = utensil.CreatedOn,
                LastModifiedOn = utensil.LastModifiedOn
            };

            return Ok(response);
        }
    }


}
