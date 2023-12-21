namespace recipesApi.Model.Response
{
    public class CookingActionResponse
    {
        public int CookingActionId { get; set; }
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
