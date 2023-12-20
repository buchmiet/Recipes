using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateRecipeIngredientAmountRequest
    {
        public int RecipeId { get; set; } // ID for existing Recipe
        public int IngredientAmountId { get; set; } // ID for existing IngredientAmount
    }

    public class CreateRecipeIngredientAmountRequestValidator : AbstractValidator<CreateRecipeIngredientAmountRequest>
    {
        private readonly IEntityService<Recipe> _recipeService;
        private readonly IEntityService<IngredientAmount> _ingredientAmountService;

        public CreateRecipeIngredientAmountRequestValidator(IEntityService<Recipe> recipeService, IEntityService<IngredientAmount> ingredientAmountService)
        {
            _recipeService = recipeService;
            _ingredientAmountService = ingredientAmountService;

            RuleFor(request => request.RecipeId)
                .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

            RuleFor(request => request.IngredientAmountId)
                .MustAsync(BeValidIngredientAmountId).WithMessage("IngredientAmountId must refer to an existing ingredient amount");
        }

        private async Task<bool> BeValidRecipeId(int recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            return recipe != null;
        }

        private async Task<bool> BeValidIngredientAmountId(int ingredientAmountId, CancellationToken cancellationToken)
        {
            var ingredientAmount = await _ingredientAmountService.GetByIdAsync(ingredientAmountId);
            return ingredientAmount != null;
        }
    }


}
