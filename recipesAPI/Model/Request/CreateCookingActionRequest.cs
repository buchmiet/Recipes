using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateCookingActionRequest
    {
        public int RecipeId { get; set; } // ID for existing Recipe
        public string Name { get; set; }
 
    }

    public class CreateCookingActionRequestValidator : AbstractValidator<CreateCookingActionRequest>
    {
        private readonly IEntityService<Recipe> _recipeService;

        public CreateCookingActionRequestValidator(IEntityService<Recipe> recipeService)
        {
            _recipeService = recipeService;

            RuleFor(request => request.RecipeId)
                .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(500).WithMessage("Name must not exceed 500 characters");

          
        }

        private async Task<bool> BeValidRecipeId(int recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            return recipe != null;
        }
    }


}
