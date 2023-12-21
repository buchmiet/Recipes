using FluentValidation;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesApi.Model.Request
{
    public class CreateRecipeRequest
    {
        public string Title { get; set; }
        public int CookingTime { get; set; }
        public int AuthorId { get; set; } // ID for existing Author
    }

    public class CreateRecipeRequestValidator : AbstractValidator<CreateRecipeRequest>
    {
        private readonly IEntityService<Author> _authorService;

        public CreateRecipeRequestValidator(IEntityService<Author> authorService)
        {
            _authorService = authorService;

            RuleFor(request => request.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(request => request.CookingTime)
                .GreaterThan(0).WithMessage("Cooking time must be positive");

            RuleFor(request => request.AuthorId)
                .MustAsync(BeValidAuthorId).WithMessage("AuthorId must refer to an existing author");
        }

        private async Task<bool> BeValidAuthorId(int authorId, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetByIdAsync(authorId);
            return author != null;
        }
    }


}
