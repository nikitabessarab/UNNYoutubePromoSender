using System.Text;

namespace UNNYoutubePromoSender;

public static class FoundContactsStore
{
    private static readonly object FileLock = new();
    private static readonly UTF8Encoding Utf8Bom = new(encoderShouldEmitUTF8Identifier: true);

    public static string AppFolder =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UNNYoutubePromoSender");

    private static string LogPath => Path.Combine(AppFolder, "found_emails_log.csv");
    public static string SnapshotPath => Path.Combine(AppFolder, "channels_snapshot.csv");

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
            sb.AppendLine("ChannelId;Title;Subscribers;ChannelUrl;FoundEmail;MailSentAtUtc");
            foreach (var x in items)
            {
                var sent = x.MailSentAtUtc.HasValue
                    ? x.MailSentAtUtc.Value.ToString("O")
                    : "";
                sb.AppendLine(string.Join(";",
                    Csv(x.ChannelId),
                    Csv(x.Title),
                    x.SubscriberCount?.ToString() ?? "",
                    Csv(x.ChannelUrl),
                    Csv(x.FoundEmail ?? ""),
                    Csv(sent)));
            }

            File.WriteAllText(SnapshotPath, sb.ToString(), Utf8Bom);
        }
    }

    /// <summary>
    /// Восстанавливает таблицу из последнего снимка (channels_snapshot.csv).
    /// </summary>
    public static IReadOnlyList<ChannelListItem> TryLoadSnapshot()
    {
        lock (FileLock)
        {
            if (!File.Exists(SnapshotPath))
                return Array.Empty<ChannelListItem>();

            string[] lines;
            try
            {
                lines = File.ReadAllLines(SnapshotPath, Utf8Bom);
            }
            catch
            {
                return Array.Empty<ChannelListItem>();
            }

            if (lines.Length < 2)
                return Array.Empty<ChannelListItem>();

            var list = new List<ChannelListItem>();
            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                var fields = ParseSemicolonCsvLine(line);
                if (fields.Count < 5)
                    continue;

                ulong? subs = null;
                if (ulong.TryParse(fields[2].Trim().Trim('"'), out var n))
                    subs = n;

                var ch = new ChannelListItem
                {
                    ChannelId = Unquote(fields[0]),
                    Title = Unquote(fields[1]),
                    SubscriberCount = subs,
                    ChannelUrl = Unquote(fields[3])
                };
                var mail = Unquote(fields[4]);
                if (!string.IsNullOrWhiteSpace(mail))
                    ch.FoundEmail = mail.Trim();

                if (fields.Count >= 6)
                {
                    var sentRaw = Unquote(fields[5]).Trim();
                    if (!string.IsNullOrEmpty(sentRaw) &&
                        DateTime.TryParse(sentRaw, null,
                            System.Globalization.DateTimeStyles.RoundtripKind, out var sentUtc))
                    {
                        ch.MailSentAtUtc = sentUtc.Kind == DateTimeKind.Unspecified
                            ? DateTime.SpecifyKind(sentUtc, DateTimeKind.Utc)
                            : sentUtc.ToUniversalTime();
                    }
                }

                list.Add(ch);
            }

            return list;
        }
    }

    private static List<string> ParseSemicolonCsvLine(string line)
    {
        var result = new List<string>();
        var i = 0;
        while (i < line.Length)
        {
            if (line[i] == ';')
            {
                result.Add("");
                i++;
                continue;
            }

            if (line[i] == '"')
            {
                i++;
                var sb = new StringBuilder();
                while (i < line.Length)
                {
                    if (line[i] == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            sb.Append('"');
                            i += 2;
                            continue;
                        }

                        i++;
                        break;
                    }

                    sb.Append(line[i]);
                    i++;
                }

                result.Add(sb.ToString());
                if (i < line.Length && line[i] == ';')
                    i++;
                continue;
            }

            var start = i;
            while (i < line.Length && line[i] != ';')
                i++;
            result.Add(line[start..i].Trim());
            if (i < line.Length && line[i] == ';')
                i++;
        }

        return result;
    }

    private static string Unquote(string s)
    {
        s = s.Trim();
        if (s.Length >= 2 && s.StartsWith('"') && s.EndsWith('"'))
            s = s[1..^1].Replace("\"\"", "\"");
        return s;
    }

    private static string Csv(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return "\"\"";
        var s = value.Replace("\"", "\"\"");
        return $"\"{s}\"";
    }
}
