using Telegram.Bot.Downloader.Youtube.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Downloader.Youtube.BotService.MessageSender;

public class ReplyKeyboardMarkups
{
    public static ValueTask<ReplyKeyboardMarkup> ForMainState(bool permission)
    {
        List<List<KeyboardButton>> keyboardButtons;
        if (permission)
        {
            keyboardButtons =
                           [
                               [
                                   new KeyboardButton("👨‍👨‍👦‍👦 Get all users"),
                                   new KeyboardButton("📣 Send all users Ads")
                               ],
                           ];
        }
        else
        {


            keyboardButtons =
            [
                [
                    new KeyboardButton("🎧 Download music")
                ],

                [
                    new KeyboardButton("📺 Download Youtube movie or video"),
                    new KeyboardButton("📸 Download Instagram video or store")
                ],
            ];
        }
        
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(keyboardButtons) { ResizeKeyboard = true };

        return ValueTask.FromResult(replyKeyboardMarkup);
    }
    
    public static ValueTask<InlineKeyboardMarkup> ForMainState()
    {
        InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            // first row
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "🎧 Download music", callbackData: Status.Music.ToString()),
            },
            // second row
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "📺 Download Youtube movie or video", callbackData: Status.Youtube.ToString()),
                InlineKeyboardButton.WithCallbackData(text: "📸 Download Instagram video or store", callbackData: Status.Instagram.ToString()),
            },
        });
        
        return ValueTask.FromResult(inlineKeyboard);
    }
    

    public static ValueTask<ReplyKeyboardMarkup> ForPhoneNumberRequest()
    {
        var keyboardButton = KeyboardButton.WithRequestContact("📱 Share phone number 📱");
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(keyboardButton) { ResizeKeyboard = true };

        return ValueTask.FromResult(replyKeyboardMarkup);
    }

    
    
    
}