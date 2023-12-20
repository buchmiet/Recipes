using FluentValidation;

namespace recipesCommon.Model.Request
{
    public class CreateUtensilRequest
    {
        public string Name { get; set; }
    }

    public class CreateUtensilRequestValidator : AbstractValidator<CreateUtensilRequest>
    {
        public CreateUtensilRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");
        }
    }

}
