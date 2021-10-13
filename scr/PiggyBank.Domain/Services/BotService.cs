using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Model;
using Newtonsoft.Json;
using PiggyBank.Common.Interfaces;
using PiggyBank.Domain.Handler.Bot;
using PiggyBank.Domain.Infrastructure;
using PiggyBank.Domain.Models.Operations;
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

        public async Task<string> ProcessUpdateCommand(Update command, string operationSnapshotJson, CancellationToken token)
        {
            var method = command.Type switch
            {
                UpdateType.Message => ProcessMessage(command.Message, operationSnapshotJson, token),
                _ => ProcessUnknownMessage(command, token)
            };

            try
            {
                return await method;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error during handler invoke");
            }

            return null;
        }

        private async Task<string> ProcessUnknownMessage(Update command, CancellationToken token)
        {
            var unknownTypeHandler = new UnknownMessageTypeHandler(_piggyContext, command, _client);
            await _piggyDispatcher.InvokeHandler(unknownTypeHandler, token);
            return null;
        }

        private async Task<string> ProcessMessage(Message commandMessage, string operationSnapshotJson, CancellationToken token)
        {
            var userId = GetUserId(commandMessage.Chat.Id, token);
            var operation = string.IsNullOrWhiteSpace(operationSnapshotJson) ? null : JsonConvert.DeserializeObject<BotOperationSnapshot>(operationSnapshotJson); 
            
            switch (commandMessage.Text)
            {
                case { } text when text.StartsWith("/start"):
                    var startHandler = new StartHandler(_identityContext, commandMessage, _client);
                    await _identityDispatcher.InvokeHandler(startHandler, token);
                    return null;
                    break;
                case { } text when text.StartsWith("/help"):
                    break;
                case { } text when  text.StartsWith("/settings"):
                    break;
                case { } text when text.Contains("➖ Add expense"):
                    var addExpenseHandler = new AddExpenseHandler(_piggyContext, commandMessage, _client, userId);
                    await _piggyDispatcher.InvokeHandler(addExpenseHandler, token);
                    operation = (BotOperationSnapshot)addExpenseHandler.Result;
                    return JsonConvert.SerializeObject(operation);
                
                case { } text when text.Contains("➕ Add income"):
                    var addIncomeHandler = new AddIncomeHandler(_piggyContext, commandMessage, _client, userId);
                    await _piggyDispatcher.InvokeHandler(addIncomeHandler, token);
                    operation = (BotOperationSnapshot)addIncomeHandler.Result;
                    return JsonConvert.SerializeObject(operation);
                
                case { } text when text.Contains("🔁 Add transfer"):
                    var addTransferHandler = new AddTransferHandler(_piggyContext, commandMessage, _client, userId);
                    await _piggyDispatcher.InvokeHandler(addTransferHandler, token);
                    operation = (BotOperationSnapshot)addTransferHandler.Result;
                    return JsonConvert.SerializeObject(operation);
            }

            return null;
        }

        private string GetUserId(long chatId, CancellationToken token)
            => _identityContext.Users.FirstOrDefault(u => u.ChatId == chatId)?.Id;
    }
}