namespace recipesApi.Model.Response
{
    public class IngredientAmountResponse
    {
        public int IngredientAmountId { get; set; }
        public int IngredientAmountTypeId { get; set; }
        public int IngredientId { get; set; }
        public float Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }   
    }

}
