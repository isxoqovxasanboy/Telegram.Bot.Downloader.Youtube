using Newtonsoft.Json;
using Telegram.Bot.Downloader.Youtube.Clients;
using Telegram.Bot.Downloader.Youtube.Entities.Models;
using Telegram.Bot.Downloader.Youtube.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using YoutubeExplode;
using File = System.IO.File;

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
                chatId: update.CallbackQuery!.Message!.Chat.Id,
                text: "Okay send link.",
                cancellationToken: cancellationToken
            );
        }

        return message;
    }


    public static async ValueTask<Message> ForMainState(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken,Client storegUser)
    {
        
        var message = await botClient.SendTextMessageAsync(
            chatId: update.Message is null ?  storegUser.TelegramId : update.Message!.Chat.Id,
            text: "Okay. Select one page",
            replyMarkup: await ReplyKeyboardMarkups.ForMainState(),
            cancellationToken: cancellationToken
        );

        return message;
    }


    public static async ValueTask<Message> SendReadyRequest(ITelegramBotClient botClient, Update update,
                                                            CancellationToken cancellationToken, string whoReady)
    {
        var message = await botClient.SendTextMessageAsync
        (
            chatId: update.Message!.Chat.Id,
            text: whoReady,
            cancellationToken: cancellationToken
        );

        return message;
    }


    //send video
    public static async ValueTask<Message> SendResultVideo(ITelegramBotClient botClient, Update update,
                                                           CancellationToken cancellationToken, string videoUrls)
    {
        var youtube = new YoutubeClient();
        Message message;

        var video = await youtube.Videos.GetAsync(videoUrls, cancellationToken);

        // Keraksiz bo'lgan nomlarni o'chiradi
        string sanitizedTitle = string.Join("_", video.Title.Split(Path.GetInvalidFileNameChars()));

        // Kelayotgan videoni kachestvasini qaytaradi
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id, cancellationToken);
        var muxedStreams = streamManifest.GetMuxedStreams().OrderByDescending(s => s.VideoQuality).ToList();

        await botClient.SendChatActionAsync
        (
            chatId: update.Message!.Chat.Id,
            chatAction: ChatAction.UploadVideo,
            cancellationToken: cancellationToken
        );

        if (muxedStreams.Any())
        {
            var streamInfo = muxedStreams.First();
            using var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(streamInfo.Url, cancellationToken);
            message = await botClient.SendVideoAsync
            (
                chatId: update.Message!.Chat.Id,
                video: InputFile.FromStream(stream),
                supportsStreaming: true,
                caption: sanitizedTitle,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            message = await botClient.SendTextMessageAsync
            (
                chatId: update.Message!.Chat.Id,
                text: "Video not found",
                cancellationToken: cancellationToken
            );
        }

        return message;
    }




    public static async ValueTask<Message> SendResultVideoByInstagram(ITelegramBotClient botClient, Update update,
                                                                      CancellationToken cancellationToken, string url)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri =
                new Uri(url),
            Headers =
            {
                { "X-RapidAPI-Key", "55986fec27msh0d7c21c836d8933p1c7cd8jsn4d78c0f6a5b0" },
                { "X-RapidAPI-Host", "instagram-downloader-download-instagram-videos-stories.p.rapidapi.com" },
            },
        };


        using var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var se = JsonConvert.DeserializeObject<Root>(body);

        var d = new HttpClient();
        var responses = d.GetAsync(se.media).Result;
        var b = responses.Content.ReadAsStream(cancellationToken);

        var message = await botClient.SendVideoAsync
        (
            chatId: update.Message!.Chat.Id,
            video: InputFile.FromStream(b),
            caption: "Done",
            cancellationToken: cancellationToken
        );

        return message;
    }



    public class Root
    {
        public string media { get;}
        public string thumbnail { get; set; }
        public string Type { get; set; }
        public string API { get; set; }
        public string title { get; set; }
    }
    
    #region Admin Commands

    //Userlani ma'lumotlarini qaytaradi
    public static async ValueTask<Message> SendUserInfoAsync(ITelegramBotClient botClient, Update update,
                                                             CancellationToken cancellationToken,
                                                             ClientService clientService)
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