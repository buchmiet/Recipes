
namespace recipesAPI.Services
{
    public interface ISearchService
    {
        Task<List<int>> Search(string searchTerm);
        Task UpdateSearchtermForRecipe(int recipeId);
    }
}