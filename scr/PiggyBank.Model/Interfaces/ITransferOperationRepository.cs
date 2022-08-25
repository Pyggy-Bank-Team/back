using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Model.Models.Entities;

namespace PiggyBank.Model.Interfaces
{
    public interface ITransferOperationRepository
    {
        Task<TransferOperation> AddAsync(TransferOperation operation, CancellationToken token);
        Task<TransferOperation> GetAsync(Guid userId, int operationId, CancellationToken token);
        IEnumerable<TransferOperation> GetAll(Guid userId);
        IQueryable<TransferOperation> GetAllAsQueryable(Guid userId);
        Task<TransferOperation>  UpdateAsync(TransferOperation operation, CancellationToken token);
    }
}