using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Model;
using Microsoft.EntityFrameworkCore;
using PiggyBank.Common.Commands.Bot;
using PiggyBank.Common.Enums;
using PiggyBank.Common.Interfaces;
using PiggyBank.Domain.Handler.Bot;
using PiggyBank.Domain.Infrastructure;
using PiggyBank.Model;
using Serilog;
using Telegram.Bot;

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

        public async Task UpdateProcessing(UpdateCommand updateCommand, CancellationToken token)
        {
            try
            {
                switch (updateCommand.Text)
                {
                    case {} defaultText when CheckForDefaultCommands(defaultText):
                        await DefaultCommandsProcessing(updateCommand, token);
                        break;
                    case {} beginText when CheckForBeginCommands(beginText):
                        updateCommand.UserId = await GetUserId(updateCommand.ChatId, token);
                        await BeginCommandsProcessing(updateCommand, token);
                        break;
                    default:
                        updateCommand.UserId = await GetUserId(updateCommand.ChatId, token);
                        await ExistOperationsProcessing(updateCommand, token);
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error during processing an update command from bot");
            }
        }

        public Task UnknownMessageTypeProcessing(UnknownMessageTypeCommand command, CancellationToken token)
        {
            var unknownMessageTypeHandler = new UnknownMessageTypeHandler(_piggyContext, command, _client);
            return _piggyDispatcher.InvokeHandler(unknownMessageTypeHandler, token);
        }

        private async Task DefaultCommandsProcessing(UpdateCommand updateCommand, CancellationToken token)
        {
            switch (updateCommand.Text)
            {
                case { } text when text.StartsWith("/start"):
                    var startHandler = new StartHandler(_identityContext, updateCommand, _client);
                    await _identityDispatcher.InvokeHandler(startHandler, token);
                    break;
                case { } text when text.StartsWith("/help"):
                    var helpHandler = new HelpHandler(_piggyContext, updateCommand, _client);
                    await _piggyDispatcher.InvokeHandler(helpHandler, token);
                    break;
                case { } text when text.StartsWith("/settings"):
                    var settingsHandler = new SettingsHandler(_piggyContext, updateCommand, _client);
                    await _piggyDispatcher.InvokeHandler(settingsHandler, token);
                    break;
            }
        }

        private async Task BeginCommandsProcessing(UpdateCommand updateCommand, CancellationToken token)
        {
            switch (updateCommand.Text)
            {
                case { } text when text.Contains("➖ Add expense"):
                    var addExpenseHandler = new AddExpenseHandler(_piggyContext, updateCommand, _client);
                    await _piggyDispatcher.InvokeHandler(addExpenseHandler, token);
                    break;
                case { } text when text.Contains("➕ Add income"):
                    var addIncomeHandler = new AddIncomeHandler(_piggyContext, updateCommand, _client);
                    await _piggyDispatcher.InvokeHandler(addIncomeHandler, token);
                    break;

                case { } text when text.Contains("🔁 Add transfer"):
                    var addTransferHandler = new AddTransferHandler(_piggyContext, updateCommand, _client);
                    await _piggyDispatcher.InvokeHandler(addTransferHandler, token);
                    break;
            }
        }

        private async Task ExistOperationsProcessing(UpdateCommand updateCommand,  CancellationToken token)
        {
            var operation = await _piggyContext.BotOperations.Where(b => b.ChatId == updateCommand.ChatId).OrderByDescending(b => b.CreatedOn).FirstAsync(token);
            
            if (operation == null)
            {
                var nullOperationHandler = new NullOperationHandler(_piggyContext, updateCommand, _client);
                await _piggyDispatcher.InvokeHandler(nullOperationHandler, token);
                return;
            }
            
            switch (operation.Stage)
            {
                case CreationStage.AmountInput:
                    var addAmountHandler = new AmountInputHandler(_piggyContext, updateCommand, _client, operation);
                    await _piggyDispatcher.InvokeHandler(addAmountHandler, token);
                    break;
                case CreationStage.AccountSelection:
                    var accountOperationHandler = new FromAccountHandler(_piggyContext, updateCommand, _client, operation);
                    await _piggyDispatcher.InvokeHandler(accountOperationHandler, token);
                    break;
                case CreationStage.CategoryOrAccountSelection when operation.Type == OperationType.Budget:
                    var categoryOperationHandler = new ToCategoryHandler(_piggyContext, updateCommand, _client, operation);
                    await _piggyDispatcher.InvokeHandler(categoryOperationHandler, token);
                    break;
                case CreationStage.CategoryOrAccountSelection when operation.Type == OperationType.Transfer:
                    var account1OperationHandler = new ToAccountHandler(_piggyContext, updateCommand, _client, operation);
                    await _piggyDispatcher.InvokeHandler(account1OperationHandler, token);
                    break;
                default:
                    _logger.Error("Unknown bot operation stage. Actually stage:{Stage}", operation.Stage);
                    break;
            }
        }
        

        private static bool CheckForDefaultCommands(string messageText)
        {
            switch (messageText)
            {
                case { } startMessage when startMessage.StartsWith("/start"):
                case { } helpMessage when helpMessage.StartsWith("/help"):
                case { } settingsMessage when settingsMessage.StartsWith("/settings"):
                    return true;
                default:
                    return false;
            }
        }

        private static bool CheckForBeginCommands(string messageText)
        {
            switch (messageText)
            {
                case { } expense when expense.Contains("➖ Add expense"):
                case { } income when income.Contains("➕ Add income"):
                case { } transfer when transfer.Contains("🔁 Add transfer"):
                    return true;
                default:
                    return false;
            }
        }
        
        private async Task<string> GetUserId(long chatId, CancellationToken token)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.ChatId == chatId, token);
            return user?.Id;
        }
    }
}