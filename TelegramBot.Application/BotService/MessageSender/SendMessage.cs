// using Telegram.Bot.Downloader.Youtube.Entities.Models;

using Telegram.Bot.Downloader.Youtube.Clients;
using Telegram.Bot.Types;
using File = System.IO.File;

// using File = System.IO.File;

namespace Telegram.Bot.Downloader.Youtube.BotService.MessageSender;

public static class SendMessage
{
    //Telefon number so'rash uchun
    public static async ValueTask<Message> ForPhoneNumberRequest(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        var message = await botClient.SendTextMessageAsync(
            chatId: update.Message!.Chat.Id,
            text:
            "📱 Telefon raqamingiz qanday? Telefon raqamingizni jo'natish uchun, quyidagi \"📱 Raqamni jo'natish\" tugmasini bosing.",
            replyMarkup: await ReplyKeyboardMarkups.ForPhoneNumberRequest(),
            cancellationToken: cancellationToken
        );

        return message;
    }

    //Service ko'rsatish
    public static async ValueTask<Message> ForMainState(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken, bool adminPermission = true)
    {
        Message message;
        if (adminPermission)
        {
            message = await botClient.SendTextMessageAsync(
                            chatId: update.Message!.Chat.Id,
                            text: "Good.What do you need?",
                            replyMarkup: await ReplyKeyboardMarkups.ForMainState(adminPermission),
                            cancellationToken: cancellationToken
                        );
        }
        else
        {

            message = await botClient.SendTextMessageAsync(
                              chatId: update.Message!.Chat.Id,
                              text: "Okay. Please only send the link",
                              replyMarkup: await ReplyKeyboardMarkups.ForMainState(adminPermission),
                              cancellationToken: cancellationToken
                          );
        }


        return message;
    }

    #region Admin Commands

    //Userlani ma'lumotlarini qaytaradi
    public static async ValueTask<Message> SendUserInfoAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, ClientService clientService)
    {
        var result = await clientService.ConvertPdf();
        Message message;

        if (string.IsNullOrWhiteSpace(result))
        {
            message = await botClient.SendTextMessageAsync(
                chatId: update.Message!.Chat.Id,
                text: "Users not fount",
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await using Stream stream = File.OpenRead(result);
            message = await botClient.SendDocumentAsync
            (
                chatId: update.Message!.Chat.Id,
                document: InputFile.FromStream(stream, "Result.pdf"),
                caption: "All users information",
                cancellationToken: cancellationToken
            );
            stream.Close();
        }
        return message;
    }

    #endregion
}