using FluentValidation;
using recipesCommon.DataAccess;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateCookingApplianceRequest
    {
        public string Name { get; set; }
    }

    public class CreateCookingApplianceRequestValidator : AbstractValidator<CreateCookingApplianceRequest>
    {
        private readonly IEntityService<CookingAppliance> _cookingApplianceService;

        public CreateCookingApplianceRequestValidator(IEntityService<CookingAppliance> cookingApplianceService)
        {
            _cookingApplianceService = cookingApplianceService;

            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .MustAsync(BeUniqueName).WithMessage("Name must be unique");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {            
            var cookingAppliance = await _cookingApplianceService.GetOneAsync(p=>p.Name.Equals(name));          
            return cookingAppliance == null;
        }
    }

}
