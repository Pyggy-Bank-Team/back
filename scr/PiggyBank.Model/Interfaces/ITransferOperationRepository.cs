using System;
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
        Task<IQueryable<TransferOperation>> GetAllAsync(Guid userId, CancellationToken token);
        Task<TransferOperation>  UpdateAsync(TransferOperation operation, CancellationToken token);
    }
}