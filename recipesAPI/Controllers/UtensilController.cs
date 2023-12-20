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

        public UtensilController(IEntityService<Utensil> utensilService)
        {
            _utensilService = utensilService;
        }

        [HttpPost]
        public async Task<ActionResult<UtensilResponse>> AddUtensil(CreateUtensilRequest request)
        {
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
