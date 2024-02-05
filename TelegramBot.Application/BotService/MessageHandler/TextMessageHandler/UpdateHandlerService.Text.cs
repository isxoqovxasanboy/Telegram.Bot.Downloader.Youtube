using Telegram.Bot.Downloader.Youtube.BotService.MessageSender;
using Telegram.Bot.Downloader.Youtube.Enums;
using Telegram.Bot.Types;

namespace Telegram.Bot.Downloader.Youtube.BotService;

public partial class UpdateHandlerService
{
    private async Task HandleTextMessageAsync(ITelegramBotClient botClient, Update update,
                                              CancellationToken cancellationToken)
    {
        var textMessage = update.Message!.Text;
        var from = update.Message.From;
        var storageUser = _clientService.GetClient(from!.Id);

        //authentication:agar user null bo'lsa registratsiya qilamiz
        storageUser = await Authentication(botClient, update, from, storageUser, cancellationToken);
        if (storageUser != null)
        {
            var state = storageUser.Status;

            //Admin panel go to
            if (storageUser!.IsAdmin)
            {
                //authorization:find admin status
                if (state == Status.Contact)
                {
                }
                else
                {
                    if ("📣 Send all users Ads" == textMessage)
                    {
                        var clients = _clientService.GetAllUsers();

                        if (clients.Count != 0)
                        {
                            foreach (var client in clients)
                            {
                                await botClient.SendPhotoAsync
                                (
                                    chatId: client.TelegramId,
                                    photo: InputFile.FromUri($"https://telegram-bot-sdk.com/img/hero-logo.png"),
                                    caption: "Bu sinov uchun qilindi",
                                    cancellationToken: cancellationToken
                                );
                            }
                        }
                    }

                    var textHandler = textMessage switch
                    {
                        "/start" => SendMessage.ForMainState(botClient, update, cancellationToken, storageUser.IsAdmin),
                        "👨‍👨‍👦‍👦 Get all users" => SendPdf(botClient, update, cancellationToken),
                        // "🎧 Download music" => "",
                        // "📺 Download Youtube movie or video" => "",
                        // "📸 Download Instagram video or store" => "",
                        _ => SendMessage.ForMainState(botClient, update, cancellationToken, storageUser.IsAdmin)
                    };

                    try
                    {
                        await textHandler;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception:" + ex.Message);
                    }
                }
            }
            else
            {
                //authorization:find users status
                switch (state)
                {
                    case Status.Inactive:
                        await SendMessage.ForPhoneNumberRequest(botClient, update, cancellationToken);
                        return;
                    case Status.Music:
                        await CommandDownloadResaltAsync(botClient, update, cancellationToken, Status.Music,
                            textMessage!);
                        return;
                    case Status.Youtube:
                        await CommandDownloadResaltAsync(botClient, update, cancellationToken, Status.Youtube,
                            textMessage!);

                        return;
                    case Status.Instagram:
                        await CommandDownloadResaltAsync(botClient, update, cancellationToken, Status.Instagram,
                            textMessage!);
                        return;

                }

                var textHandler = textMessage switch
                {
                    "/start" => await CommandForPhoneNumberRequest(botClient, update, cancellationToken),
                    "📺 Download Youtube movie or video" => await CommandWhoSendMessage(botClient, update, cancellationToken, Status.Youtube),
                    "🎧 Download music" => await CommandWhoSendMessage(botClient, update, cancellationToken, Status.Music),
                    "📸 Download Instagram video or store" => await CommandWhoSendMessage(botClient, update, cancellationToken, Status.Instagram),
                    _ => await CommandForPhoneNumberRequest(botClient, update, cancellationToken),
                };




            }
        }
    }


}