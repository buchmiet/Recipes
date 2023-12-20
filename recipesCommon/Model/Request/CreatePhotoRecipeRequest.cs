using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

public class CreatePhotoRecipeRequest
{
    public int PhotoId { get; set; } // ID for existing Photo
    public int RecipeId { get; set; } // ID for existing Recipe
    public int Position { get; set; }
}

public class CreatePhotoRecipeRequestValidator : AbstractValidator<CreatePhotoRecipeRequest>
{
    private readonly IEntityService<Photo> _photoService;
    private readonly IEntityService<Recipe> _recipeService;

    public CreatePhotoRecipeRequestValidator(IEntityService<Photo> photoService, IEntityService<Recipe> recipeService)
    {
        _photoService = photoService;
        _recipeService = recipeService;

        RuleFor(request => request.PhotoId)
            .MustAsync(BeValidPhotoId).WithMessage("PhotoId must refer to an existing photo");

        RuleFor(request => request.RecipeId)
            .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");

        RuleFor(request => request.Position)
            .GreaterThanOrEqualTo(0).WithMessage("Position must be non-negative");
    }

    private async Task<bool> BeValidPhotoId(int photoId, CancellationToken cancellationToken)
    {
        var photo = await _photoService.GetByIdAsync(photoId);
        return photo != null;
    }

    private async Task<bool> BeValidRecipeId(int recipeId, CancellationToken cancellationToken)
    {
        var recipe = await _recipeService.GetByIdAsync(recipeId);
        return recipe != null;
    }
}

