using System.Text.Json;
using Telegram.Bot.Downloader.Youtube.Entities.Common;
using File = System.IO.File;

namespace Telegram.Bot.Downloader.Youtube.DataLayer.FileSet;

public class FileSet<TData> : List<TData> where TData : class, IEntity
{
    private readonly string _directoryName;
    private readonly string _path;
    public string? FullFilePath;

    public FileSet(string name, string directoryName = "Database")
    {
        var fileName = $"{name.ToLower()}.json";
        _directoryName = Path.Combine(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName, directoryName);
        _path = Path.Combine(_directoryName, fileName);
        FullFilePath = _directoryName;

        EnsurePathExists();
    }

    public void Initialize()
    {
        var fileStream = File.Open(_path, FileMode.Open);
        var reader = new StreamReader(fileStream);
        var rawData = reader.ReadToEnd();

        if (!string.IsNullOrWhiteSpace(rawData))
        {
            Clear();
            var previousData = JsonSerializer.Deserialize<List<TData>>(rawData);
            AddRange(previousData);
        }

        fileStream.Flush();
        fileStream.Close();
    }

    public void SaveChanges()
    {
        var fileStream = File.Open(_path, FileMode.Open);
        fileStream.SetLength(0);
        JsonSerializer.Serialize(fileStream, this);

        fileStream.Flush();
        fileStream.Close();
    }

    public new void Add(TData item)
    {
        item.Id = Count == 0 ? 1 : this.Last().Id + 1;
        base.Add(item);
    }

    private void EnsurePathExists()
    {
        Directory.CreateDirectory(_directoryName);
        File.Open(_path, FileMode.OpenOrCreate).Close();
    }
}