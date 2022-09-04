namespace Common.Results.Models.Dto.Dashboard
{
    public class ChartByCategoryDto
    {
        public int CategoryId { get; set; }
        
        public string CategoryTitle { get; set; }
        
        public string CategoryHexColor { get; set; }
        
        public decimal Amount { get; set; }
        
        public string Currency { get; set; }
    }
}