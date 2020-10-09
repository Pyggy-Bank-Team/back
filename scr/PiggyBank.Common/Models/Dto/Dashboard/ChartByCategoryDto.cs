﻿namespace PiggyBank.Common.Models.Dto.Dashboard
{
    public class ChartByCategoryDto
    {
        public int CategoryId { get; set; }
        
        public string CategoryName { get; set; }
        
        public string CategoryHexColor { get; set; }
        
        public decimal Amount { get; set; }
    }
}