using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateTagRequest
    {
        public string Name { get; set; }
    }

    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        private readonly IEntityService<Tag> _tagService;
        public CreateTagRequestValidator(IEntityService<Tag> tagService)
        {
            _tagService = tagService;
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                   .MustAsync(BeUniqueName).WithMessage("Tag name must be unique");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            // Sprawdź, czy istnieje już tag o podanej nazwie
            var existingTag = await _tagService.GetOneAsync(p=>p.Name.Equals(name));

            // Jeśli istnieje, to nazwa nie jest unikalna
            return existingTag == null;
        }

    }

}
