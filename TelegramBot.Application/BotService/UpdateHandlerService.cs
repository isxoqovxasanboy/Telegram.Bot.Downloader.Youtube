using Telegram.Bot.Downloader.Youtube.BotService.MessageSender;
using Telegram.Bot.Downloader.Youtube.Clients;
using Telegram.Bot.Downloader.Youtube.DataLayer.Context;
using Telegram.Bot.Downloader.Youtube.Enums;
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

    private async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // var textMessage = update.CallbackQuery!.Data;
        // var from = update.CallbackQuery!.Message!.Chat;
        // var storageUser = _clientService.GetClient(from!.Id);
        // var calBack = update.CallbackQuery.Message.Text;
        //
        // if (storageUser.Status == Status.Active  &&
        //     textMessage!.StartsWith(Status.Youtube.ToString()))
        // {
        //   await SendMessage.ForMainState(botClient, update, cancellationToken, false);
        //    _clientService.UpdateClientUserStatus(from!.Id, Status.Youtube);
        // }
        // else if (storageUser.Status == Status.Active  &&
        //          textMessage!.StartsWith(Status.Music.ToString()))
        // {
        //     await SendMessage.ForMainState(botClient, update, cancellationToken, false);
        //     _clientService.UpdateClientUserStatus(from!.Id, Status.Music);
        //
        // }
        // else if (storageUser.Status == Status.Active  &&
        //          textMessage!.StartsWith(Status.Instagram.ToString()))
        // {
        //     await SendMessage.ForMainState(botClient, update, cancellationToken, false);
        //     _clientService.UpdateClientUserStatus(from!.Id, Status.Instagram);
        // }
        // else
        // {
        //     await SendMessage.ForMainState(botClient, update, cancellationToken,storageUser);
        //     _clientService.UpdateClientUserStatus(from!.Id, storageUser.Status);
        // }


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