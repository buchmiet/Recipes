namespace recipesApi.Model.Response
{
    public class IngredientResponse
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }

}
