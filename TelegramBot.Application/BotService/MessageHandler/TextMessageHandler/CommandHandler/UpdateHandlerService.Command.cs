using Telegram.Bot.Downloader.Youtube.BotService.MessageSender;
using Telegram.Bot.Downloader.Youtube.Clients;
using Telegram.Bot.Downloader.Youtube.Enums;
using Telegram.Bot.Types;
using Client = Telegram.Bot.Downloader.Youtube.Entities.Models.Client;

namespace Telegram.Bot.Downloader.Youtube.BotService;

public partial class UpdateHandlerService
{
    // public async ValueTask<Message> CommandDownloadMusicRequest(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    // {
    //     
    //     var message = await 
    //     
    // }


    public async ValueTask<Message> CommandDownloadResaltAsync(ITelegramBotClient botClient, Update update,
                                                               CancellationToken cancellationToken, Status status,
                                                               string urlToken)
    {
        Message message;
        if (urlToken.StartsWith("https://youtu.be/"))
        {
            message = await SendMessage.SendResultVideo(botClient, update, cancellationToken, urlToken);
            _clientService.UpdateClientUserStatus(update.Message!.Chat.Id, Status.Active);
        }
        else
        {
            message = new Message();
        }


        return message;
    }


    //Commandalar To'plami update users Status 
    private async ValueTask<Message> CommandWhoSendMessage(ITelegramBotClient botClient, Update update,
                                                           CancellationToken cancellationToken, Status status)
    {
        Message message;

        switch (status)
        {
            case Status.Youtube:
                message = await SendMessage.SendReadyRequest(botClient, update, cancellationToken,
                    $"Send {status.ToString()} link");

                _clientService.UpdateClientUserStatus(update.Message!.Chat.Id, Status.Youtube);
                break;
            case Status.Music:
                message = await SendMessage.SendReadyRequest(botClient, update, cancellationToken,
                    $"Send {status.ToString()} link");
                _clientService.UpdateClientUserStatus(update.Message!.Chat.Id, Status.Music);
                break;
            default:
                message = await SendMessage.SendReadyRequest(botClient, update, cancellationToken,
                    $"Send {status.ToString()} link");
                _clientService.UpdateClientUserStatus(update.Message!.Chat.Id, Status.Instagram);
                break;
        }

        return message;
    }


    //Userlarni registratsiya qilish uchun
    private async ValueTask<Client?> Authentication(ITelegramBotClient botClient, Update update, User? from,
                                                    Client? storageUser, CancellationToken cancellationToken)
    {
        return storageUser ??= await Register(botClient, update, from, cancellationToken);
    }

    //Pdf file jo'natadi
    private async ValueTask<Message> SendPdf(ITelegramBotClient botClient, Update update,
                                             CancellationToken cancellationToken)
    {
        var message =
            await SendMessage.SendUserInfoAsync(botClient, update, cancellationToken, (ClientService)_clientService);

        return message;
    }

    private ValueTask<Client> Register(ITelegramBotClient botClient, Update update, User? from,
                                       CancellationToken cancellationToken)
    {
        var client = new Client()
        {
            TelegramId = from!.Id,
            FirstName = from.FirstName,
            LastName = from.LastName,
            Username = from.Username,
            IsAdmin = false
        };

        var clientEntry = _clientService.Add(client);

        return ValueTask.FromResult(clientEntry);
    }


    public async ValueTask<Message> CommandForPhoneNumberRequest(ITelegramBotClient botClient, Update update,
                                                                 CancellationToken cancellationToken)
    {
        var storageUser = _clientService.GetClient(update.Message!.From!.Id);

        if (storageUser == null)
            return await SendMessage.ForPhoneNumberRequest(botClient, update, cancellationToken);
        else
            return await SendMessage.ForMainState(botClient, update, cancellationToken, false);
    }
}