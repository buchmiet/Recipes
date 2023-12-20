using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreateRecipeTagRelationRequest
    {
        public int RecipeId { get; set; } // ID for existing Recipe
        public int TagId { get; set; } // ID for existing Tag
    }

    public class CreateRecipeTagRelationRequestValidator : AbstractValidator<CreateRecipeTagRelationRequest>
    {
        private readonly IEntityService<Recipe> _recipeService;
        private readonly IEntityService<Tag> _tagService;

        public CreateRecipeTagRelationRequestValidator(IEntityService<Recipe> recipeService, IEntityService<Tag> tagService)
        {
            _recipeService = recipeService;
            _tagService = tagService;

            RuleFor(request => request.RecipeId)
                .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

            RuleFor(request => request.TagId)
                .MustAsync(BeValidTagId).WithMessage("TagId must refer to an existing tag");
        }

        private async Task<bool> BeValidRecipeId(int recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            return recipe != null;
        }

        private async Task<bool> BeValidTagId(int tagId, CancellationToken cancellationToken)
        {
            var tag = await _tagService.GetByIdAsync(tagId);
            return tag != null;
        }
    }


}
