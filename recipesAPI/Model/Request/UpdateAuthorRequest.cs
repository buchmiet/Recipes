﻿using FluentValidation;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesAPI.Model.Request
{
    public class UpdateAuthorRequest
    {
        public string AuthorName { get; set; }
    }

    public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        private readonly IEntityService<Author> _authorService;

        public UpdateAuthorRequestValidator(IEntityService<Author> authorService)
        {
            _authorService = authorService;

            RuleFor(request => request.AuthorName)
                .NotEmpty().WithMessage("Author name is required")
                .MaximumLength(200).WithMessage("Author name must not exceed 200 characters")
                .MustAsync(BeUniqueName).WithMessage("Author name must be unique");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetOneAsync(p => p.AuthorName.Equals(name));
            return author == null;
        }
    }

}
