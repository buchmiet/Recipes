using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("CookingAction")]
    public class CookingActionController : ControllerBase
    {
        private readonly IEntityService<CookingAction> _cookingActionService;

        public CookingActionController(IEntityService<CookingAction> cookingActionService)
        {
            _cookingActionService = cookingActionService;
        }

        [HttpPost]
        public async Task<ActionResult<CookingActionResponse>> AddCookingAction(CreateCookingActionRequest request)
        {
            var cookingAction = new CookingAction
            {
                RecipeId = request.RecipeId,
                Name = request.Name,
                Position = request.Position,
                CreatedOn = DateTime.UtcNow
            };

            await _cookingActionService.AddAsync(cookingAction);

            var response = new CookingActionResponse
            {
                CookingActionId = cookingAction.CookingActionId,
                RecipeId = cookingAction.RecipeId,
                Name = cookingAction.Name,
                Position = cookingAction.Position,
                CreatedOn = cookingAction.CreatedOn
            };

            return Ok(response);
        }
    }

}
