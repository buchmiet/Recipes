using FluentValidation;

namespace recipesCommon.Model.Request
{
    public class CreateTagRequest
    {
        public string Name { get; set; }
    }

    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        public CreateTagRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        }
    }

}
