using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Telegram.Bot.Downloader.Youtube.DataLayer.Context;
using Telegram.Bot.Downloader.Youtube.Entities.Models;
using Telegram.Bot.Downloader.Youtube.Enums;

namespace Telegram.Bot.Downloader.Youtube.Clients;

public class ClientService(AppDataContext context) : IClientService
{
    private string _pdfPath = String.Empty;

    public Client Add(Client client)
    {
        var storageClient = context.Clients.FirstOrDefault(x => x.TelegramId == client.TelegramId);

        if (storageClient == null)
        {
            context.Clients.Add(client);

            context.SaveChanges();
            return client;
        }
        else
        {
            return storageClient;
        }
    }

    public bool DeleteClient(long id)
    {
        var storageClient = context.Clients.FirstOrDefault(x => x.TelegramId == id);

        if (storageClient == null)
        {
            return false;
        }
        else
        {
            context.Clients.Remove(storageClient);
            context.SaveChanges();
            return true;
        }
    }

    public Client GetClient(long id)
    {
        var storageUser = context.Clients.FirstOrDefault(x => x.TelegramId == id);

        return storageUser;
    }


    public Client UpdateClientPhoneNumber(long telegramId, string phoneNumber)
    {
        var storageUser = context.Clients.FirstOrDefault(x => x.TelegramId == telegramId);

        if (storageUser == null)
        {
            return storageUser;
        }
        else
        {
            storageUser.PhoneNumber = phoneNumber;
            storageUser.Status = Status.Active;

            context.Clients.Remove(storageUser);
            context.Clients.Add(storageUser);
            context.SaveChanges();

            return storageUser;
        }
    }

    public Client UpdateClientUserStatus(long telegramId, Status status)
    {
        var storageUser = context.Clients.FirstOrDefault(x => x.TelegramId == telegramId);

        if (storageUser == null)
        {
            return storageUser ?? new Client();
        }
        else
        {
            storageUser.Status = status;
            context.Clients.Remove(storageUser);
            context.Clients.Add(storageUser);
            context.SaveChanges();

            return storageUser;
        }
    }

    public bool IsOwner(Client client)
    {
        var owner = context.Clients.FirstOrDefault(clients => clients.IsAdmin = client.IsAdmin);

        if (owner is null)
        {
            return false;
        }

        return true;
    }

    public List<Client> GetAllUsers() =>
        context.Clients.Where(client => client.IsAdmin == false).ToList();

    //Convertatsiyani amalga oshirib javob qaytaradi
    private async Task<bool> IsConvertClientsAsync()
    {
        var res = GetAllUsers();

        string deserializeAllClients = "";
        if (res.Count != 0)
        {
            res.ForEach(client => deserializeAllClients += $"{client.Id}  {client.TelegramId}  {client.Username} " +
                                                           $"{client.FirstName}  {client.LastName}  {client.PhoneNumber} \n");
        }

        _pdfPath = Path.Combine(context.Clients.FullFilePath!, "Result.pdf");

        if (!Directory.Exists(context.Clients.FullFilePath!))
        {
            Directory.CreateDirectory(context.Clients.FullFilePath!);
        }

        if (!string.IsNullOrEmpty(deserializeAllClients))
        {
            try
            {
                QuestPDF.Settings.License = LicenseType.Community;
                // code in your main method
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(20));

                        page.Header()
                          .Text("Hello PDF!")
                          .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                        page.Content()
                          .PaddingVertical(1, Unit.Centimetre)
                          .Column(x =>
                          {
                              x.Spacing(20);

                              x.Item().Text(deserializeAllClients);
                          });

                        page.Footer()
                          .AlignCenter()
                          .Text(x =>
                          {
                              x.Span("Page ");
                              x.CurrentPageNumber();
                          });
                    });
                })
                .GeneratePdf(_pdfPath);

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(true);
            }
        }
        else
        {
            return await Task.FromResult(false);
        }
    }


    //Userlarni ma'lumotlarini pdf ga aylantiradi
    public async Task<string> ConvertPdf()
    {
        var isTrue = await Task.Run(IsConvertClientsAsync);

        if (isTrue)
        {
            return _pdfPath;
        }
        else
        {
            return "";
        }
    }
}