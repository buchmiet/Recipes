using FluentValidation;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesApi.Model.Request
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
        private readonly IEntityService<RecipeCookingAppliance> _recipeCookingApplianceService;

        public CreateRecipeCookingApplianceRequestValidator(IEntityService<Recipe> recipeService, IEntityService<CookingAppliance> cookingApplianceService, IEntityService<RecipeCookingAppliance> recipeCookingApplianceService)
        {
            _recipeService = recipeService;
            _cookingApplianceService = cookingApplianceService;
            _recipeCookingApplianceService=recipeCookingApplianceService;

            RuleFor(request => request.RecipeId)
                .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

            RuleFor(request => request.CookingApplianceId)
                .MustAsync(BeValidCookingApplianceId).WithMessage("CookingApplianceId must refer to an existing cooking appliance");
            RuleFor(request => new KeyValuePair<int,int> ( request.RecipeId, request.CookingApplianceId ))
            .MustAsync(BeUniqueRecipeCookingAppliancePair).WithMessage("Recipe-CookingAppliance pair must be unique");
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

        private async Task<bool> BeUniqueRecipeCookingAppliancePair(KeyValuePair<int, int> RrecipeCookingAppliancId, CancellationToken cancellationToken)
        {
            var existingPair = await _recipeCookingApplianceService.GetOneAsync(p =>
               p.RecipeId == RrecipeCookingAppliancId.Key && p.CookingApplianceId == RrecipeCookingAppliancId.Value);

            // Jeśli para już istnieje, to nie jest unikalna
            return existingPair == null;
        }

    }


}
