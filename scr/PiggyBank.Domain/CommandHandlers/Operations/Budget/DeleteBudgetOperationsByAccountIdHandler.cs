using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Commands.Operations.Budget;
using Common.Results.Operations.Budget;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.CommandHandlers.Operations.Budget
{
    public class DeleteBudgetOperationsByAccountIdHandler : IRequestHandler<DeleteBudgetOperationsByAccountIdCommand, DeleteBudgetOperationsByAccountIdResult>
    {
        private readonly IBudgetOperationRepository _repository;
        private readonly IMediator _mediator;

        public DeleteBudgetOperationsByAccountIdHandler(IBudgetOperationRepository repository, IMediator mediator)
            => (_repository, _mediator) = (repository, mediator);

        public async Task<DeleteBudgetOperationsByAccountIdResult> Handle(DeleteBudgetOperationsByAccountIdCommand request, CancellationToken cancellationToken)
        {
            var allBudgetOperations = _repository.GetAll(request.ModifiedBy);
            
            foreach (var budgetOperation in allBudgetOperations.Where(b => b.AccountId == request.AccountId))
            {
                var deleteBudgetOperationCommand = new DeleteBudgetOperationCommand
                {
                    Id = budgetOperation.Id,
                    ModifiedBy = request.ModifiedBy,
                    ModifiedOn = request.ModifiedOn
                };

                _ = await _mediator.Send(deleteBudgetOperationCommand, cancellationToken);
            }

            return new DeleteBudgetOperationsByAccountIdResult();
        }
    }
}