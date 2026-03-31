using System.Text;

namespace UNNYoutubePromoSender;

public static class FoundContactsStore
{
    private static readonly object FileLock = new();
    private static readonly UTF8Encoding Utf8Bom = new(encoderShouldEmitUTF8Identifier: true);

    public static string AppFolder =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UNNYoutubePromoSender");

    private static string LogPath => Path.Combine(AppFolder, "found_emails_log.csv");
    private static string SnapshotPath => Path.Combine(AppFolder, "channels_snapshot.csv");

    public static void AppendFound(ChannelListItem channel, string email)
    {
        Directory.CreateDirectory(AppFolder);
        lock (FileLock)
        {
            if (!File.Exists(LogPath))
                File.WriteAllText(LogPath, "UtcTime;ChannelId;Title;Email;ChannelUrl\r\n", Utf8Bom);

            var line = string.Join(";",
                Csv(DateTime.UtcNow.ToString("O")),
                Csv(channel.ChannelId),
                Csv(channel.Title),
                Csv(email),
                Csv(channel.ChannelUrl));

            File.AppendAllText(LogPath, line + "\r\n", Utf8Bom);
        }
    }

    public static void SaveSnapshot(IReadOnlyList<ChannelListItem> items)
    {
        Directory.CreateDirectory(AppFolder);
        lock (FileLock)
        {
            var sb = new StringBuilder();
            sb.AppendLine("ChannelId;Title;Subscribers;ChannelUrl;FoundEmail");
            foreach (var x in items)
            {
                sb.AppendLine(string.Join(";",
                    Csv(x.ChannelId),
                    Csv(x.Title),
                    x.SubscriberCount?.ToString() ?? "",
                    Csv(x.ChannelUrl),
                    Csv(x.FoundEmail ?? "")));
            }

            File.WriteAllText(SnapshotPath, sb.ToString(), Utf8Bom);
        }
    }

    private static string Csv(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return "\"\"";
        var s = value.Replace("\"", "\"\"");
        return $"\"{s}\"";
    }
}
