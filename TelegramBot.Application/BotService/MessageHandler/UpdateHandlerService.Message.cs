using Telegram.Bot.Downloader.Youtube.BotService.MessageSender;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Downloader.Youtube.BotService;

public partial class UpdateHandlerService
{
    private async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var messageHandler = update.Message!.Type switch
        {
            MessageType.Text => HandleTextMessageAsync(botClient, update, cancellationToken),
            MessageType.Location => HandleLocationAsync(botClient, update, cancellationToken),
            MessageType.Contact => HandleContactAsync(botClient, update, cancellationToken),
            // MessageType.Audio => HandleAudioAsync(botClient, update, cancellationToken),
            // MessageType.Sticker => HandlerStrikerAsync(botClient, update, cancellationToken),
            // MessageType.Photo => HandlePhotoAsync(botClient, update, cancellationToken),
            // MessageType.Dice => HandleDiceAsync(botClient, update, cancellationToken),
            // MessageType.Document => HandleDocumentAsync(botClient, update, cancellationToken),
            // MessageType.Game => HandleGameAsync(botClient, update, cancellationToken),
            // MessageType.Invoice => HandleInvoiceAsync(botClient, update, cancellationToken),
            // MessageType.Poll => HandlePollAsync(botClient, update, cancellationToken),
            // MessageType.Voice => HandleVoiceAsync(botClient, update, cancellationToken),
            // MessageType.VideoNote => HandleVideoNote(botClient, update, cancellationToken),
            // MessageType.WebAppData => HandleWebAppDataAsync(botClient, update, cancellationToken),
            // MessageType.Video => HandleVideoAsync(botClient, update, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, update, cancellationToken)
        };

        try
        {
            await messageHandler;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }
    }

    private Task HandleUnknownMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task HandleContactAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var from = update.Message!.From;
        var contact = update.Message.Contact;
        _clientService.UpdateClientPhoneNumber(from!.Id, contact!.PhoneNumber);

        await SendMessage.ForMainState(botClient, update, cancellationToken, false);
    }

    private Task HandleLocationAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

}