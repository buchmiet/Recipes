using FluentValidation;

namespace recipesAPI.Model.Request
{
    public class UpdateRecipeRequest
    {
        public string Title { get; set; }
        public int CookingTime { get; set; }
   
    }

    public class UpdateRecipeRequestValidator : AbstractValidator<UpdateRecipeRequest>
    {
        public UpdateRecipeRequestValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters")
                .When(request => request.Title != null);

            RuleFor(request => request.CookingTime)
                .GreaterThan(0).WithMessage("Cooking time must be positive");
              
        }
    }

}
