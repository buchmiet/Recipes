using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateIngredientTypeRequest
    {
        public string Name { get; set; }
    }

    public class CreateIngredientTypeRequestValidator : AbstractValidator<CreateIngredientTypeRequest>
    {
        private IEntityService<IngredientType> _ingredientTypeService;
        public CreateIngredientTypeRequestValidator(IEntityService<IngredientType> ingredientTypeService)
        {
            _ingredientTypeService = ingredientTypeService;
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters")
                   .MustAsync(BeUniqueName).WithMessage("Name must be unique");
        }


        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            // Sprawdź, czy istnieje już rodzaj składnika o podanej nazwie
            var existingIngredientType = await _ingredientTypeService.GetOneAsync(p=>p.Name.Equals(name));

            // Jeśli istnieje, to nazwa nie jest unikalna
            return existingIngredientType == null;
        }

    }

}
