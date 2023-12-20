namespace recipesCommon.Model.Response
{
    public class RecipeTagRelationResponse
    {
        public int RecipeTagRelationId { get; set; }
        public int RecipeId { get; set; }
        public int TagId { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}
