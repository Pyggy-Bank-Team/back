using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Models.Dto.Operations;
using PiggyBank.Common.Queries;
using PiggyBank.Model;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Domain.Queries.Operations
{
    public class GetTransferOperationQuery : BaseQuery<TransferDto>
    {
        private readonly GetOperationQuery _query;

        public GetTransferOperationQuery(PiggyContext context, GetOperationQuery query) : base(context)
            => _query = query;

        public override Task<TransferDto> Invoke(CancellationToken token)
            => GetRepository<TransferOperation>().Where(t => !t.IsDeleted
                                                             && t.CreatedBy == _query.UserId
                                                             && t.Id == _query.OperationId)
                .Select(t => new TransferDto
                {
                    Id = t.Id,
                    Type = t.Type,
                    Amount = t.Amount,
                    Comment = t.Comment,
                    Date = t.OperationDate,
                    FromId = t.From,
                    ToId = t.To
                })
                .FirstOrDefaultAsync(token);
    }
}