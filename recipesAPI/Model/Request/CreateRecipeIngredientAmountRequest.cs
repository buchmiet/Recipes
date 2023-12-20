using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateRecipeIngredientAmountRequest
    {
        public int RecipeId { get; set; } // ID for existing Recipe
        public int IngredientId { get; set; } // ID for existing IngredientAmount

        public float IngredientAmount { get; set;}
    }

    public class CreateRecipeIngredientAmountRequestValidator : AbstractValidator<CreateRecipeIngredientAmountRequest>
    {
        private readonly IEntityService<Recipe> _recipeService;
        private readonly IEntityService<Ingredient> _ingredientService;
        private readonly IEntityService<RecipeIngredient> _recipeIngredientService; 

        public CreateRecipeIngredientAmountRequestValidator(IEntityService<Recipe> recipeService, IEntityService<Ingredient> ingredientService, IEntityService<RecipeIngredient> recipeIngredientService)
        {
            _recipeService = recipeService;          
            _ingredientService = ingredientService;
            _recipeIngredientService = recipeIngredientService;

            RuleFor(request => request.RecipeId)
                .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

            RuleFor(request => request.IngredientId)
                .MustAsync(BeValidIngredientId).WithMessage("IngredientId must refer to an existing ingredient");
            RuleFor(request => request.IngredientAmount).  
                GreaterThan(0).WithMessage("IngredientAmount must be greater than 0");
            RuleFor(request => new KeyValuePair<int, int>(request.RecipeId, request.IngredientId))
           .MustAsync(BeUniqueRecipeIngredientPair).WithMessage("Recipe-Ingredient pair must be unique");
        }

        private async Task<bool> BeValidRecipeId(int recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            return recipe != null;
        }

        private async Task<bool> BeValidIngredientId(int ingredientId, CancellationToken cancellationToken)
        {
            var ingredientAmount = await _ingredientService.GetByIdAsync(ingredientId);
            return ingredientAmount != null;
        }


        private async Task<bool> BeUniqueRecipeIngredientPair(KeyValuePair<int, int> recipeIngredientId, CancellationToken cancellationToken)
        {
            var existingPair = await _recipeIngredientService.GetOneAsync(p =>
                p.RecipeId == recipeIngredientId.Key && p.IngredientId == recipeIngredientId.Value);

            // If the pair already exists, it's not unique
            return existingPair == null;
        }
    }


}
