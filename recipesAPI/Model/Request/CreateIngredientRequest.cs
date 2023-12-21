using FluentValidation;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesApi.Model.Request
{
    public class CreateIngredientRequest
    {
        public string IngredientName { get; set; }
        public int Type { get; set; } // ID for IngredientType
        public int AmountType { get; set; }
    }

    public class CreateIngredientRequestValidator : AbstractValidator<CreateIngredientRequest>
    {
        private readonly IEntityService<IngredientType> _ingredientTypeService;
        private readonly IEntityService<Ingredient> _ingredientService;
        private readonly IEntityService<IngredientAmountType> _ingredientAmountTypeService;

        public CreateIngredientRequestValidator(IEntityService<IngredientType> ingredientTypeService, IEntityService<Ingredient> ingredientService, IEntityService<IngredientAmountType> ingredientAmountTypeService)
        {
            _ingredientTypeService = ingredientTypeService;

            _ingredientService = ingredientService;
            _ingredientAmountTypeService = ingredientAmountTypeService;

            RuleFor(request => request.IngredientName)
                .NotEmpty().WithMessage("Ingredient name is required")
                .MaximumLength(200).WithMessage("Ingredient name must not exceed 200 characters")
                .MustAsync(BeUniqueIngredientName).WithMessage("Ingredient name must be unique");

            RuleFor(request => request.Type)
                .GreaterThan(0).WithMessage("Type must be a valid Ingredient Type ID")
                .MustAsync(BeValidIngredientTypeId).WithMessage("Type must refer to an existing Ingredient Type");

            RuleFor(request => request.AmountType)
                .GreaterThan(0).WithMessage("Amount Type must be a valid Ingredient Amount Type ID")
                .MustAsync(BeValidIngredientAmountTypeId).WithMessage("Type must refer to an existing Ingredient Type");

        }

        private async Task<bool> BeValidIngredientTypeId(int ingredientTypeId, CancellationToken cancellationToken)
        {
            var ingredientType = await _ingredientTypeService.GetByIdAsync(ingredientTypeId);
            return ingredientType != null;
        }

        private async Task<bool> BeValidIngredientAmountTypeId(int ingredientAmountTypeId, CancellationToken cancellationToken)
        {
            var ingredientType = await _ingredientAmountTypeService.GetByIdAsync(ingredientAmountTypeId);
            return ingredientType != null;
        }

        private async Task<bool> BeUniqueIngredientName(string name, CancellationToken cancellationToken)
        {
            // Sprawdź, czy istnieje już składnik o podanej nazwie
            var ingredient = await _ingredientService.GetOneAsync(p => p.IngredientName.Equals(name));

            // Jeśli istnieje, to nazwa nie jest unikalna
            return ingredient == null;
        }

    }


}
