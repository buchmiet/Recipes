using recipesApi.DataAccess;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesAPI.Services
{
    public class SearchService : ISearchService
    {

        private readonly IUnitOfWork _unitOfWork;
        public SearchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public static HashSet<string> getWords(string wyrazenie)
        {
            if (string.IsNullOrWhiteSpace(wyrazenie))
            {
                return new HashSet<string>();
            }
            var przecinki = new List<char> { ' ', '-', '.', '/', '\n', '\r' }.ToArray();
            var wszystkieslowa = wyrazenie.Split(przecinki);
            return new HashSet<string>(wszystkieslowa.Select(w => w.ToLower()));
        }

        public async Task UpdateSearchtermForRecipe(int recipeId)
        {
            HashSet<string> searchTerms = new();
            var recipeService = new EntityService<Recipe>(_unitOfWork);
            var utensilService = new EntityService<Utensil>(_unitOfWork);
            var authorService = new EntityService<Author>(_unitOfWork);
            var recipeUtensilService = new EntityService<RecipeUtensil>(_unitOfWork);
            var cookingApplianceService = new EntityService<CookingAppliance>(_unitOfWork);
            var recipeCookingApplianceService = new EntityService<RecipeCookingAppliance>(_unitOfWork);
            var tagService = new EntityService<Tag>(_unitOfWork);
            var recipeTagRelationService = new EntityService<RecipeTagRelation>(_unitOfWork);
            var ingredientService = new EntityService<Ingredient>(_unitOfWork);
            var recipeIngredientService = new EntityService<RecipeIngredient>(_unitOfWork);
            var ingredientTypeService = new EntityService<IngredientType>(_unitOfWork);

            var recipe = await recipeService.GetByIdAsync(recipeId);
           
           searchTerms.UnionWith(getWords(recipe.Title));

            var author = await authorService.GetByIdAsync(recipe.AuthorId);
            searchTerms.UnionWith(getWords(author.AuthorName));

            var recipeUtensils = (await recipeUtensilService.GetAllAsync(p => p.RecipeId == recipeId)).Select(p => p.UtensilId);
            var utensilNames = (await utensilService.GetAllAsync(p => recipeUtensils.Contains(p.UtensilId))).Select(p => p.Name).ToHashSet();
            searchTerms.UnionWith(utensilNames);

            var recipeCookingAppliances = (await recipeCookingApplianceService.GetAllAsync(p => p.RecipeId == recipeId)).Select(p => p.CookingApplianceId);
            var CookingApplianceNames = (await cookingApplianceService.GetAllAsync(p => recipeCookingAppliances.Contains(p.CookingApplianceId))).Select(p => p.Name).ToHashSet();
            searchTerms.UnionWith(CookingApplianceNames);

            var recipeTags = (await recipeTagRelationService.GetAllAsync(p => p.RecipeId == recipeId)).Select(p => p.TagId);
            var tagsNames = (await tagService.GetAllAsync(p => recipeTags.Contains(p.TagId))).Select(p => p.Name).ToHashSet();
            searchTerms.UnionWith(tagsNames);

            var recipeIngredients = (await recipeIngredientService.GetAllAsync(p => p.RecipeId == recipeId)).Select(p => p.IngredientId);
            var ingredientsUsed = await ingredientService.GetAllAsync(p => recipeIngredients.Contains(p.IngredientId));
            var ingredientNames = ingredientsUsed.Select(p => p.IngredientName).ToHashSet();
            searchTerms.UnionWith(ingredientNames);
            var ingredientTypesIds = ingredientsUsed.Select(p => p.IngredientAmountTypeId).ToList();
            var ingredientTypeNames = (await ingredientTypeService.GetAllAsync(p => ingredientTypesIds.Contains(p.IngredientTypeId))).Select(p => p.Name);

            searchTerms.UnionWith(ingredientTypeNames);

            recipe = await recipeService.GetByIdAsync(recipeId);
            recipe.SearchTerms = string.Join(' ', searchTerms).ToLower();
            await recipeService.UpdateAsync(recipe);
        }




        public async Task<List<int>> Search(string searchTerm)
        {
            var recipeService = new EntityService<Recipe>(_unitOfWork);
            HashSet<string> words = getWords(searchTerm);
            Dictionary<int, string> allRecipes = (await recipeService.GetAllAsync())
                                                                    .ToDictionary(p => p.RecipeId, q => q.SearchTerms);

            var foundIds = new HashSet<int>(allRecipes.Keys);

            foreach (var word in words)
            {

                var idsWithWord = allRecipes.Where(p => p.Value.Contains(word))
                                            .Select(p => p.Key)
                                            .ToHashSet();


                foundIds.IntersectWith(idsWithWord);
            }

            return foundIds.ToList();
        }



    }
}
