using System;
using PiggyBank.Common.Enums;

namespace PiggyBank.Common.Models.Dto.Operations
{
    public abstract class OperationBase
    {
        public int Id { get; set; }
        
        public decimal Amount { get; set; }
        
        public OperationType Type { get; set; }
        
        public DateTime Date { get; set; }
    }
}