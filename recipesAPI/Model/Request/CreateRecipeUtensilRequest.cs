using FluentValidation;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesApi.Model.Request
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
        private readonly IEntityService<RecipeUtensil> _recipeUtensilService; // Added service for Recipe-Utensil relationship

        public CreateRecipeUtensilRequestValidator(
            IEntityService<Recipe> recipeService,
            IEntityService<Utensil> utensilService,
            IEntityService<RecipeUtensil> recipeUtensilService) // Added parameter
        {
            _recipeService = recipeService;
            _utensilService = utensilService;
            _recipeUtensilService = recipeUtensilService; // Initialize the new service

            RuleFor(request => request.RecipeId)
                .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

            RuleFor(request => request.UtensilId)
                .MustAsync(BeValidUtensilId).WithMessage("UtensilId must refer to an existing utensil");

            RuleFor(request => new KeyValuePair<int, int>(request.RecipeId, request.UtensilId))
                .MustAsync(BeUniqueRecipeUtensilPair).WithMessage("Recipe-Utensil pair must be unique");
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

        private async Task<bool> BeUniqueRecipeUtensilPair(KeyValuePair<int, int> recipeUtensilId, CancellationToken cancellationToken)
        {
            var existingPair = await _recipeUtensilService.GetOneAsync(p =>
                p.RecipeId == recipeUtensilId.Key && p.UtensilId == recipeUtensilId.Value);

            // If the pair already exists, it's not unique
            return existingPair == null;
        }
    }


}
