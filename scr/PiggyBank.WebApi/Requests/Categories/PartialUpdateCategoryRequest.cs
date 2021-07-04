namespace PiggyBank.WebApi.Requests.Categories
{
    public class PartialUpdateCategoryRequest
    {
        public string Title { get; set; }

        public string HexColor { get; set; }

        //PB-72
        //public CategoryType? Type { get; set; }
        
        public bool? IsArchived { get; set; }
    }
}