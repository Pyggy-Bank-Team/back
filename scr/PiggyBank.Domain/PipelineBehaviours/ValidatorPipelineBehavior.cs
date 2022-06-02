using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Results;
using FluentValidation;
using MediatR;

namespace PiggyBank.Domain.PipelineBehaviours
{
    public class ValidatorPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> 
                                                                                                         where TResponse : BaseResult, new()
    {
        private readonly IValidator<TRequest> _validator;

        public ValidatorPipelineBehavior(IValidator<TRequest> validator)
            => _validator = validator;
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validatorResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
                return new TResponse { ErrorCode = ErrorCodes.InvalidRequest, Messages = validatorResult.Errors.Select(e => e.ErrorMessage).ToArray() };

            return await next();
        }
    }
}