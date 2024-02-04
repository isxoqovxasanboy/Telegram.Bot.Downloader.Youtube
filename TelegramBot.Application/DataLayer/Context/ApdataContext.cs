using Telegram.Bot.Downloader.Youtube.DataLayer.FileSet;
using Telegram.Bot.Downloader.Youtube.Entities.Models;

namespace Telegram.Bot.Downloader.Youtube.DataLayer.Context;

public class AppDataContext
{
    public FileSet<Client> Clients { get; init; } = new(name: nameof(Client));

    public AppDataContext()
    {
        Initialize();
    }

    private void Initialize()
    {
        Clients.Initialize();
    }

    public void SaveChanges()
    {
        Clients.SaveChanges();
    }


}



