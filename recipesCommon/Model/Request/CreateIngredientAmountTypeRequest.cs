using FluentValidation;

namespace recipesCommon.Model.Request
{
    public class CreateIngredientAmountTypeRequest
    {
        public string UnitName { get; set; }
    }

    public class CreateIngredientAmountTypeRequestValidator : AbstractValidator<CreateIngredientAmountTypeRequest>
    {
        public CreateIngredientAmountTypeRequestValidator()
        {
            RuleFor(request => request.UnitName)
                .NotEmpty().WithMessage("Unit name is required")
                .MaximumLength(100).WithMessage("Unit name must not exceed 100 characters");
        }
    }

}
