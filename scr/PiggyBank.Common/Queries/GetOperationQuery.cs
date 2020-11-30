using System;

namespace PiggyBank.Common.Queries
{
    public class GetOperationQuery
    {
        public Guid UserId { get; set; }

        public int OperationId { get; set; }
    }
}