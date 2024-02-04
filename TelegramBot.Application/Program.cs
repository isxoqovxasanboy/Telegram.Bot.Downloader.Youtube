
// using Telegram.Bot.Downloader.Youtube.BotService;
// using Telegram.Bot.Polling;
// using Telegram.Bot.Types.Enums;

using Telegram.Bot.Downloader.Youtube.BotService;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Downloader.Youtube;
//string accountSid = Environment.GetEnvironmentVariable($"SKe52dd271f4605ce4d74d2602d51bc2dd")
//string authToken = Environment.GetEnvironmentVariable("SvbHj49G4GsIRPbL6e7qaEo22XFh4m45")

public partial class Program
{
    public static async Task Main(string[] args)
    {
        string token = $"6185034889:AAH9Eki4eHD6vvQG1dvg4dPoUAJpSfbP1fk";
        TelegramBotClient botClient = new TelegramBotClient(token);
        using CancellationTokenSource cts = new();

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };

        UpdateHandlerService updateHandlerService = new UpdateHandlerService();
        botClient.StartReceiving
        (
            updateHandler: updateHandlerService.HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        await cts.CancelAsync();
    }

    private static Task HandlePollingErrorAsync(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();





    }
}
