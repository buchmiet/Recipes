namespace recipesCommon.Model.Response
{
    public class PhotoRecipeResponse
    {
        public int PhotoRecipeId { get; set; }
        public int PhotoId { get; set; }
        public int RecipeId { get; set; }
        public int Position { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
