using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateIngredientAmountRequest
    {
        public int IngredientAmountTypeId { get; set; }
        public int IngredientId { get; set; }
        public float Amount { get; set; }
    }

    public class CreateIngredientAmountRequestValidator : AbstractValidator<CreateIngredientAmountRequest>
    {
        private readonly IEntityService<Ingredient> _ingredientService;

        public CreateIngredientAmountRequestValidator(IEntityService<Ingredient> ingredientService)
        {
            _ingredientService = ingredientService;

            RuleFor(request => request.IngredientAmountTypeId)
                .GreaterThan(0).WithMessage("Ingredient Amount Type ID is required and must be positive");

            RuleFor(request => request.IngredientId)
                .GreaterThan(0).WithMessage("Ingredient ID is required and must be positive")
                .MustAsync(BeValidIngredientId).WithMessage("IngredientId must refer to an existing ingredient");

            RuleFor(request => request.Amount)
                .GreaterThan(0).WithMessage("Amount must be positive");
        }

        private async Task<bool> BeValidIngredientId(int ingredientId, CancellationToken cancellationToken)
        {
            var ingredient = await _ingredientService.GetByIdAsync(ingredientId);
            return ingredient != null;
        }
    }


}
