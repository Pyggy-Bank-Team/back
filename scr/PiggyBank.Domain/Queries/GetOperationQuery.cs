using System;

namespace PiggyBank.Domain.Queries
{
    public class GetOperationQuery
    {
        public Guid UserId { get; set; }

        public int OperationId { get; set; }
    }
}