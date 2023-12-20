namespace recipesCommon.Model.Response
{
    public class RecipeResponse
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public int CookingTime { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
     
    }

}
