using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PiggyBank.Common.Interfaces;
using PiggyBank.Domain.Handler.Bot;
using PiggyBank.Model;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PiggyBank.Domain.Services
{
    public class BotService : ServiceBase, IBotService
    {
        private readonly ILogger _logger;
        private readonly ITelegramBotClient _client;

        public BotService(PiggyContext context, ILogger logger, ITelegramBotClient client) : base(context, logger)
            => (_logger, _client) = (logger, client);

        //Use session for save temp state of operations. This why I added ASP.NET package to Domain
        public async Task ProcessUpdateCommand(Update command, ISession session, CancellationToken token)
        {
            var method = command.Type switch
            {
                UpdateType.Message => ProcessMessage(command.Message, token),
                UpdateType.EditedMessage => ProcessEditedMessage(command.EditedMessage, token),
                _ => ProcessUnknownMessage(command, token)
            };

            try
            {
                await method;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error during handler invoke");
                throw; 
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
            var handler = commandMessage.Text switch
            {
                string text when text.StartsWith("/start") => new StartHandler(new PiggyContext(), commandMessage, _client),
                _ => null
            };
        }
    }
}