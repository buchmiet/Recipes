using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateUtensilRequest
    {
        public string Name { get; set; }
    }

    public class CreateUtensilRequestValidator : AbstractValidator<CreateUtensilRequest>
    {
        private IEntityService<Utensil> _utensilService;
        public CreateUtensilRequestValidator(IEntityService<Utensil> utensilService)
        {
            _utensilService = utensilService;
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters")
                 .MustAsync(BeUniqueName).WithMessage("Name must be unique");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            // Sprawdź, czy istnieje już naczynie o podanej nazwie
            var existingUtensil = await _utensilService.GetOneAsync(p=>p.Name.Equals(name));

            // Jeśli istnieje, to nazwa nie jest unikalna
            return existingUtensil == null;
        }

    }

}
