using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateIngredientRequest
    {
        public string IngredientName { get; set; }
        public int Type { get; set; } // ID for IngredientType
    }

    public class CreateIngredientRequestValidator : AbstractValidator<CreateIngredientRequest>
    {
        private readonly IEntityService<IngredientType> _ingredientTypeService;

        public CreateIngredientRequestValidator(IEntityService<IngredientType> ingredientTypeService)
        {
            _ingredientTypeService = ingredientTypeService;

            RuleFor(request => request.IngredientName)
                .NotEmpty().WithMessage("Ingredient name is required")
                .MaximumLength(200).WithMessage("Ingredient name must not exceed 200 characters");

            RuleFor(request => request.Type)
                .GreaterThan(0).WithMessage("Type must be a valid Ingredient Type ID")
                .MustAsync(BeValidIngredientTypeId).WithMessage("Type must refer to an existing Ingredient Type");
        }

        private async Task<bool> BeValidIngredientTypeId(int ingredientTypeId, CancellationToken cancellationToken)
        {
            var ingredientType = await _ingredientTypeService.GetByIdAsync(ingredientTypeId);
            return ingredientType != null;
        }
    }


}
