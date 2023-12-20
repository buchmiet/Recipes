using FluentValidation;

namespace recipesCommon.Model.Request
{
    public class CreatePhotoRequest
    {
        public string Address { get; set; }
    }

    public class CreatePhotoRequestValidator : AbstractValidator<CreatePhotoRequest>
    {
        public CreatePhotoRequestValidator()
        {
            RuleFor(request => request.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters");
        }
    }

}
