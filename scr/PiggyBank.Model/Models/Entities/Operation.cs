using System;
using PiggyBank.Common.Enums;

namespace PiggyBank.Model.Models.Entities
{
    public class Operation : EntityModifiedBase
    {
        public string Comment { get; set; }

        public OperationType Type { get; set; }

        public bool IsDeleted { get; set; }

        public string Snapshot { get; set; }
        
        public DateTime OperationDate { get; set; }
    }
}
