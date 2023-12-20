using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateIngredientAmountTypeRequest
    {
        public string UnitName { get; set; }
    }

    public class CreateIngredientAmountTypeRequestValidator : AbstractValidator<CreateIngredientAmountTypeRequest>
    {
        private IEntityService<IngredientAmountType> _ingredientAmountTypeService;
        public CreateIngredientAmountTypeRequestValidator(IEntityService<IngredientAmountType> ingredientAmountTypeService)
        {
            _ingredientAmountTypeService = ingredientAmountTypeService;
            RuleFor(request => request.UnitName)
                .NotEmpty().WithMessage("Unit name is required")
                .MaximumLength(100).WithMessage("Unit name must not exceed 100 characters")
                  .MustAsync(BeUniqueUnitName).WithMessage("Unit name must be unique");
        }

        private async Task<bool> BeUniqueUnitName(string unitName, CancellationToken cancellationToken)
        {
            // Sprawdź, czy istnieje już rodzaj jednostki miary o podanej nazwie
            var existingUnit = await _ingredientAmountTypeService.GetOneAsync(p=>p.UnitName.Equals(unitName));

            // Jeśli istnieje, to nazwa nie jest unikalna
            return existingUnit == null;
        }

    }

}
