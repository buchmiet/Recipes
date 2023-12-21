using FluentValidation;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesApi.Model.Request
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
        private readonly IEntityService<RecipeTagRelation> _recipeTagService; // Added service for Recipe-Tag relationship

        public CreateRecipeTagRelationRequestValidator(
            IEntityService<Recipe> recipeService,
            IEntityService<Tag> tagService,
            IEntityService<RecipeTagRelation> recipeTagService) // Added parameter
        {
            _recipeService = recipeService;
            _tagService = tagService;
            _recipeTagService = recipeTagService; // Initialize the new service

            RuleFor(request => request.RecipeId)
                .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

            RuleFor(request => request.TagId)
                .MustAsync(BeValidTagId).WithMessage("TagId must refer to an existing tag");

            RuleFor(request => new KeyValuePair<int, int>(request.RecipeId, request.TagId))
          .MustAsync(BeUniqueRecipeTagPair).WithMessage("Recipe-Tag pair must be unique");
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

        private async Task<bool> BeUniqueRecipeTagPair(KeyValuePair<int, int> recipeTagId, CancellationToken cancellationToken)
        {
            var existingPair = await _recipeTagService.GetOneAsync(p =>
                p.RecipeId == recipeTagId.Key && p.TagId == recipeTagId.Value);

            // If the pair already exists, it's not unique
            return existingPair == null;
        }
    }


}
