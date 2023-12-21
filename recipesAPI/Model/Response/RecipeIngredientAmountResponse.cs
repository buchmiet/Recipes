namespace recipesApi.Model.Response
{
    public class RecipeIngredientAmountResponse
    {
        public int RecipeIngredientAmountId { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public float Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }

}
