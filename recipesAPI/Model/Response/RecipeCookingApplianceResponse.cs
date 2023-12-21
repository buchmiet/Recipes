namespace recipesApi.Model.Response
{
    public class RecipeCookingApplianceResponse
    {
        public int RecipeCookingApplianceId { get; set; }
        public int RecipeId { get; set; }
        public int CookingApplianceId { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
