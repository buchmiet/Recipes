using FluentValidation;

namespace recipesCommon.Model.Request
{
    public class CreateIngredientTypeRequest
    {
        public string Name { get; set; }
    }

    public class CreateIngredientTypeRequestValidator : AbstractValidator<CreateIngredientTypeRequest>
    {
        public CreateIngredientTypeRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");
        }
    }

}
