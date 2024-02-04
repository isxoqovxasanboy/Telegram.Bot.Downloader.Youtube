using Telegram.Bot.Downloader.Youtube.Clients;
using Telegram.Bot.Downloader.Youtube.DataLayer.Context;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Downloader.Youtube.BotService;

public partial class UpdateHandlerService : IUpdateHandler
{
    private IClientService _clientService = null!;

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        AppDataContext context = new AppDataContext();
        _clientService = new ClientService(context);

        var updateHandler = update.Type switch
        {
            UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
            UpdateType.EditedMessage => HandleEditedMessageAsync(botClient, update, cancellationToken),
            UpdateType.CallbackQuery => HandleCallbackQueryAsync(botClient, update, cancellationToken),
            UpdateType.InlineQuery => HandleInlineQueryAsync(botClient, update, cancellationToken),
            _ => HandleUnknownUpdateAsync(botClient, update, cancellationToken),
        };



        try
        {
            await updateHandler;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }



    private Task HandleUnknownUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private Task HandleInlineQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private Task HandleCallbackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private Task HandleEditedMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}