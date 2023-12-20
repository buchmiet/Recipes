using FluentValidation;

namespace recipesCommon.Model.Request
{
    public class CreateAuthorRequest
    {
        public string AuthorName { get; set; }
    }

    public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorRequestValidator()
        {
            RuleFor(request => request.AuthorName)
                .NotEmpty().WithMessage("Author name is required")
                .MaximumLength(200).WithMessage("Author name must not exceed 200 characters");
        }
    }

}
