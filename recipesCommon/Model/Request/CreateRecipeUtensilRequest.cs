using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateRecipeUtensilRequest
    {
        public int RecipeId { get; set; } // ID for existing Recipe
        public int UtensilId { get; set; } // ID for existing Utensil
    }

    public class CreateRecipeUtensilRequestValidator : AbstractValidator<CreateRecipeUtensilRequest>
    {
        private readonly IEntityService<Recipe> _recipeService;
        private readonly IEntityService<Utensil> _utensilService;

        public CreateRecipeUtensilRequestValidator(IEntityService<Recipe> recipeService, IEntityService<Utensil> utensilService)
        {
            _recipeService = recipeService;
            _utensilService = utensilService;

            RuleFor(request => request.RecipeId)
                .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

            RuleFor(request => request.UtensilId)
                .MustAsync(BeValidUtensilId).WithMessage("UtensilId must refer to an existing utensil");
        }

        private async Task<bool> BeValidRecipeId(int recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            return recipe != null;
        }

        private async Task<bool> BeValidUtensilId(int utensilId, CancellationToken cancellationToken)
        {
            var utensil = await _utensilService.GetByIdAsync(utensilId);
            return utensil != null;
        }
    }


}
