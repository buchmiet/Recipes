using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateRecipeCookingApplianceRequest
    {
        public int RecipeId { get; set; } // ID for existing Recipe
        public int CookingApplianceId { get; set; } // ID for existing CookingAppliance
    }

    public class CreateRecipeCookingApplianceRequestValidator : AbstractValidator<CreateRecipeCookingApplianceRequest>
    {
        private readonly IEntityService<Recipe> _recipeService;
        private readonly IEntityService<CookingAppliance> _cookingApplianceService;

        public CreateRecipeCookingApplianceRequestValidator(IEntityService<Recipe> recipeService, IEntityService<CookingAppliance> cookingApplianceService)
        {
            _recipeService = recipeService;
            _cookingApplianceService = cookingApplianceService;

            RuleFor(request => request.RecipeId)
                .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

            RuleFor(request => request.CookingApplianceId)
                .MustAsync(BeValidCookingApplianceId).WithMessage("CookingApplianceId must refer to an existing cooking appliance");
        }

        private async Task<bool> BeValidRecipeId(int recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            return recipe != null;
        }

        private async Task<bool> BeValidCookingApplianceId(int cookingApplianceId, CancellationToken cancellationToken)
        {
            var cookingAppliance = await _cookingApplianceService.GetByIdAsync(cookingApplianceId);
            return cookingAppliance != null;
        }
    }


}
