using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Queries;
using Common.Results.AppSettings;
using Common.Results.Models.Dto;
using MediatR;
using PiggyBank.Model.Interfaces;

namespace PiggyBank.Domain.Handlers.AppSettings
{
    public class GetAppSettingsHandler : IRequestHandler<GetAppSettingsQuery, GetAppSettingsResult>
    {
        private readonly IAppSettingsRepository _repository;

        public GetAppSettingsHandler(IAppSettingsRepository repository)
            => _repository = repository;

        public async Task<GetAppSettingsResult> Handle(GetAppSettingsQuery request, CancellationToken cancellationToken)
        {
            var appSettings = await _repository.GetAsync(request.UserId, cancellationToken);

            if (appSettings == null)
                return new GetAppSettingsResult { ErrorCode = ErrorCodes.NotFound };

            return new GetAppSettingsResult
            {
                Data = new AppSettingsDto
                {
                    BaseCurrency = appSettings.BaseCurrency,
                    Locale = appSettings.Locale,
                    UserId = appSettings.UserId,
                    TelegramChatId = appSettings.TelegramChatId
                }
            };
        }
    }
}