using Telegram.Bot.Downloader.Youtube.Entities.Common;
using Telegram.Bot.Downloader.Youtube.Enums;

namespace Telegram.Bot.Downloader.Youtube.Entities.Models;

public class Client : Auditable, IEntity
{
    public int Id { get; set; }
    public long TelegramId { get; set; }
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public string? LanguageCode { get; set; }
    public string? LastBasketProduct { get; set; }
    public Status Status { get; set; }

    public bool IsAdmin { get; set; }
}