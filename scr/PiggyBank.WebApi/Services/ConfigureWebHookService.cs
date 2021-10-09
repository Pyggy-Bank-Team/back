using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PiggyBank.WebApi.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace PiggyBank.WebApi.Services
{
    public class ConfigureWebHookService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly BotOptions _options;

        public ConfigureWebHookService(IOptions<BotOptions> options, IServiceProvider serviceProvider)
            => (_options, _serviceProvider) = (options.Value, serviceProvider);
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var telegramClient = _serviceProvider.GetService<ITelegramBotClient>();

            var webhookInfo = await telegramClient.GetWebhookInfoAsync(cancellationToken);
            var webhookUrl = $"{_options.ServerUrl}/api/telegram/update";

            //If webhook url has been already updated, then do nothing 
            if (!string.IsNullOrEmpty(webhookInfo.Url) && webhookInfo.Url == webhookUrl)
                return;

            await telegramClient.SetWebhookAsync(webhookUrl, allowedUpdates: Array.Empty<UpdateType>(), cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}