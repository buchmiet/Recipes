using FluentValidation;

namespace recipesCommon.Model.Request
{
    public class CreateCookingApplianceRequest
    {
        public string Name { get; set; }
    }

    public class CreateCookingApplianceRequestValidator : AbstractValidator<CreateCookingApplianceRequest>
    {
        public CreateCookingApplianceRequestValidator()
        {
            // Validates that the Name is not null, empty, and has a maximum length.
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        }
    }

}
