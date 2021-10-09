using System;
using System.Threading;
using System.Threading.Tasks;
using PiggyBank.Common.Interfaces;
using PiggyBank.Model;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PiggyBank.Domain.Services
{
    public class BotService : ServiceBase, IBotService
    {
        private readonly ITelegramBotClient _client;

        public BotService(PiggyContext context, ILogger logger, ITelegramBotClient client) : base(context, logger)
            => _client = client;

        public async Task ProcessUpdateCommand(Update command, CancellationToken token)
        {
            var handle = command.Type switch
            {
                UpdateType.Message => ProcessMessage(command.Message, token),
                UpdateType.EditedMessage => ProcessEditedMessage(command.EditedMessage, token),
                _ => ProcessUnknownMessage(command, token)
            };

            try
            {
                await handle;
            }
            catch (Exception e)
            {
                
            }
        }

        private Task ProcessUnknownMessage(Update command, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        private Task ProcessEditedMessage(Message commandEditedMessage, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        private Task ProcessMessage(Message commandMessage, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}