using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.Model;
using Microsoft.AspNetCore.Http;
using PiggyBank.Common.Interfaces;
using PiggyBank.Domain.Handler.Bot;
using PiggyBank.Domain.Infrastructure;
using PiggyBank.Model;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PiggyBank.Domain.Services
{
    public class BotService : IBotService
    {
        private readonly PiggyContext _piggyContext;
        private readonly IdentityContext _identityContext;
        private readonly ILogger _logger;
        private readonly ITelegramBotClient _client;
        private readonly HandlerDispatcher _piggyDispatcher;
        private readonly HandlerDispatcher _identityDispatcher;

        public BotService(PiggyContext piggyContext, IdentityContext identityContext, ILogger logger, ITelegramBotClient client)
        {
            _piggyContext = piggyContext;
            _identityContext = identityContext;
            _logger = logger;
            _client = client;

            _piggyDispatcher = new HandlerDispatcher(piggyContext, logger);
            _identityDispatcher = new HandlerDispatcher(identityContext, logger);
        }

        //Use session for save temp state of operations. This why I added ASP.NET package to Domain
        public async Task ProcessUpdateCommand(Update command, ISession session, CancellationToken token)
        {
            var method = command.Type switch
            {
                UpdateType.Message => ProcessMessage(command.Message, session, token),
                UpdateType.EditedMessage => ProcessEditedMessage(command.EditedMessage, session, token),
                _ => ProcessUnknownMessage(command, session, token)
            };

            try
            {
                await method;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error during handler invoke");
            }
        }

        private Task ProcessUnknownMessage(Update command, ISession session, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        private Task ProcessEditedMessage(Message commandEditedMessage, ISession session, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        private async Task ProcessMessage(Message commandMessage, ISession session, CancellationToken token)
        {
            switch (commandMessage.Text)
            {
                case { } text when text.StartsWith("/start"):
                    var startHandler = new StartHandler(_identityContext, commandMessage, _client);
                    await _identityDispatcher.InvokeCompletedHandler<StartHandler, Message>(startHandler, token);
                    break;
                case { } text when text.StartsWith("/help"):
                    break;
                case { } text when  text.StartsWith("/settings"):
                    break;
                case { } text when text.Contains("➖ Add expense"):
                    var addExpenseHandler = new AddExpenseHandler(_piggyContext, commandMessage, _client);
                    await _piggyDispatcher.InvokeCompletedHandler<AddExpenseHandler, Message>(addExpenseHandler, token);
                    break;
                case { } text when double.TryParse(text, out var amount):
                    break;
            }
        }
    }
}