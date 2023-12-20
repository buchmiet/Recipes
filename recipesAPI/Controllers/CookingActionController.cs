using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using System.ComponentModel.DataAnnotations;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("CookingAction")]
    public class CookingActionController : ControllerBase
    {
        private readonly IEntityService<CookingAction> _cookingActionService;
        private readonly IValidator<CreateCookingActionRequest> _validator;

        public CookingActionController(IEntityService<CookingAction> cookingActionService, IValidator<CreateCookingActionRequest> validator)
        {
            _cookingActionService = cookingActionService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<CookingActionResponse>> AddCookingAction(CreateCookingActionRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var cookingAction = new CookingAction
            {
                RecipeId = request.RecipeId,
                Name = request.Name,
               
                CreatedOn = DateTime.UtcNow
            };

            var pos = (await _cookingActionService.GetAllAsync(p => p.RecipeId == request.RecipeId));
            if (pos != null)
            {
                cookingAction.Position = pos.Count();
            }

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
