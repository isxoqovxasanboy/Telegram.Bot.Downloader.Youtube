using Telegram.Bot.Downloader.Youtube.Entities.Models;
using Telegram.Bot.Downloader.Youtube.Enums;

namespace Telegram.Bot.Downloader.Youtube.Clients;

public interface IClientService
{
    Client Add(Client client);
    Client UpdateClientPhoneNumber(long telegramId, string phoneNumber);
    Client UpdateClientUserStatus(long telegramId, Status status);
    bool DeleteClient(long id);
    Client GetClient(long id);

    List<Client> GetAllUsers();
}