using Common.Results.Models.Dto;

namespace Common.Results.Categories
{
    public class AddCategoryResult : BaseResult
    {
        public CategoryDto Data { get; set; }
    }
}