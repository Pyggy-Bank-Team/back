using System;
using Common.Enums;

namespace Common.Results.Models.Dto.Operations
{
    public abstract class OperationBase
    {
        public int Id { get; set; }
        
        public decimal Amount { get; set; }
        
        public OperationType Type { get; set; }
        
        public DateTime Date { get; set; }
        
        public string Comment { get; set; }
    }
}