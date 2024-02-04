using Telegram.Bot.Downloader.Youtube.BotService.MessageSender;
using Telegram.Bot.Downloader.Youtube.Clients;
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


    public async ValueTask<Message> CommandForPhoneNumberRequest(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var storageUser = _clientService.GetClient(update.Message!.From!.Id);

        if (storageUser == null)
            return await SendMessage.ForPhoneNumberRequest(botClient, update, cancellationToken);
        else
            return await SendMessage.ForMainState(botClient, update, cancellationToken, false);
    }

}