using System.Text.Json;

namespace UNNYoutubePromoSender;

/// <summary>
/// Локальный кеш каналов, найденных через YouTube Data API — снижает расход квоты на повторные channels.list.
/// </summary>
public static class YoutubeSearchCacheStore
{
    private static readonly object FileLock = new();
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private const int FileVersion = 1;

    private static string CachePath => Path.Combine(FoundContactsStore.AppFolder, "youtube_search_cache.json");

    public static Dictionary<string, CachedYoutubeChannel> LoadDictionary()
    {
        lock (FileLock)
        {
            try
            {
                if (!File.Exists(CachePath))
                    return new Dictionary<string, CachedYoutubeChannel>(StringComparer.Ordinal);

                var json = File.ReadAllText(CachePath);
                var root = JsonSerializer.Deserialize<CacheFileRoot>(json, JsonOptions);
                if (root?.Channels == null || root.Channels.Count == 0)
                    return new Dictionary<string, CachedYoutubeChannel>(StringComparer.Ordinal);

                return new Dictionary<string, CachedYoutubeChannel>(root.Channels, StringComparer.Ordinal);
            }
            catch
            {
                return new Dictionary<string, CachedYoutubeChannel>(StringComparer.Ordinal);
            }
        }
    }

    public static void SaveDictionary(Dictionary<string, CachedYoutubeChannel> channels)
    {
        Directory.CreateDirectory(FoundContactsStore.AppFolder);
        lock (FileLock)
        {
            var root = new CacheFileRoot { Version = FileVersion, Channels = channels };
            var json = JsonSerializer.Serialize(root, JsonOptions);
            File.WriteAllText(CachePath, json);
        }
    }

    /// <summary>
    /// Добавляет в кеш каналы из снимка таблицы, если их ещё нет (без страны/языка — только базовые поля).
    /// </summary>
    public static void ImportMissingFromSnapshot(IReadOnlyList<ChannelListItem> rows)
    {
        if (rows.Count == 0)
            return;

        var dict = LoadDictionary();
        var changed = false;
        foreach (var row in rows)
        {
            if (string.IsNullOrEmpty(row.ChannelId))
                continue;
            if (dict.ContainsKey(row.ChannelId))
                continue;
            dict[row.ChannelId] = CachedYoutubeChannel.FromListItem(row);
            changed = true;
        }

        if (changed)
            SaveDictionary(dict);
    }

    private sealed class CacheFileRoot
    {
        public int Version { get; set; }
        public Dictionary<string, CachedYoutubeChannel> Channels { get; set; } = new();
    }
}
