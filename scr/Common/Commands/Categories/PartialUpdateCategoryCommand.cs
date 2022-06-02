namespace Common.Commands.Categories
{
    public class PartialUpdateCategoryCommand : BaseModifiedCommand
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string HexColor { get; set; }

        //PB-72
        //public CategoryType? Type { get; set; }

        public bool? IsArchived { get; set; }
    }
}
