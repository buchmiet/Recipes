namespace recipesApi.Model.Response
{
    public class AuthorResponse
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }

}
