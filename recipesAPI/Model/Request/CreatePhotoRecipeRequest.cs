using FluentValidation;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;

public class CreatePhotoRecipeRequest
{
    public int PhotoId { get; set; } // ID for existing Photo
    public int RecipeId { get; set; } // ID for existing Recipe
 
}

public class CreatePhotoRecipeRequestValidator : AbstractValidator<CreatePhotoRecipeRequest>
{
    private readonly IEntityService<Photo> _photoService;
    private readonly IEntityService<Recipe> _recipeService;
    private readonly IEntityService<PhotoRecipe> _photoRecipeService;

    public CreatePhotoRecipeRequestValidator(IEntityService<Photo> photoService, IEntityService<Recipe> recipeService, IEntityService<PhotoRecipe> photoRecipeService)
    {
        _photoService = photoService;
        _recipeService = recipeService;
        _photoRecipeService = photoRecipeService;

        RuleFor(request => request.PhotoId)
            .MustAsync(BeValidPhotoId).WithMessage("PhotoId must refer to an existing photo");

        RuleFor(request => request.RecipeId)
            .MustAsync(BeValidRecipeId).WithMessage("RecipeId must refer to an existing recipe");
        RuleFor(request => new KeyValuePair<int,int>( request.PhotoId, request.RecipeId ))
           .MustAsync(BeUniquePhotoRecipePair).WithMessage("Photo-Recipe pair must be unique");


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

    private async Task<bool> BeUniquePhotoRecipePair(KeyValuePair<int,int> photoidrecipeid, CancellationToken cancellationToken)
    {
        // Sprawdź, czy istnieje już para PhotoId i RecipeId w bazie danych
        var existingPair = await _photoRecipeService.GetOneAsync(p =>
            p.PhotoId == photoidrecipeid.Key && p.RecipeId == photoidrecipeid.Value);

        // Jeśli para już istnieje, to nie jest unikalna
        return existingPair == null;
    }

}

