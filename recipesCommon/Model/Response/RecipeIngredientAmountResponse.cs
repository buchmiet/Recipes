namespace recipesCommon.Model.Response
{
    public class RecipeIngredientAmountResponse
    {
        public int RecipeIngredientAmountId { get; set; }
        public int RecipeId { get; set; }
        public int IngredientAmountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }

}
