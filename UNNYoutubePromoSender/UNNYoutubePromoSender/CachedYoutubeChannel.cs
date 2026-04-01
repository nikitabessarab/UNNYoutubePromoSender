namespace UNNYoutubePromoSender;

/// <summary>
/// Метаданные канала из API (или импорт из снимка), чтобы не запрашивать channels.list повторно.
/// </summary>
public sealed class CachedYoutubeChannel
{
    public string ChannelId { get; set; } = "";

    public string Title { get; set; } = "";

    public ulong SubscriberCount { get; set; }

    public string ChannelUrl { get; set; } = "";

    public string? DefaultLanguage { get; set; }

    public string? Country { get; set; }

    /// <summary>Начало описания для эвристики «русскоязычный».</summary>
    public string? DescriptionSnippet { get; set; }

    public bool PassesRussianChannelHeuristic()
    {
        var lang = DefaultLanguage;
        if (!string.IsNullOrEmpty(lang) && lang.StartsWith("ru", StringComparison.OrdinalIgnoreCase))
            return true;
        if (ContainsCyrillic(Title))
            return true;
        if (ContainsCyrillic(DescriptionSnippet))
            return true;
        return false;
    }

    public bool IsRussiaCountry() =>
        string.Equals(Country, "RU", StringComparison.OrdinalIgnoreCase);

    private static bool ContainsCyrillic(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return false;
        foreach (var c in text)
        {
            if (c is >= '\u0400' and <= '\u04FF')
                return true;
        }

        return false;
    }

    public ChannelListItem ToChannelListItem() =>
        new()
        {
            ChannelId = ChannelId,
            Title = Title,
            SubscriberCount = SubscriberCount,
            ChannelUrl = ChannelUrl
        };

    public static CachedYoutubeChannel FromListItem(ChannelListItem item) =>
        new()
        {
            ChannelId = item.ChannelId,
            Title = item.Title,
            SubscriberCount = item.SubscriberCount ?? 0,
            ChannelUrl = item.ChannelUrl,
            DefaultLanguage = null,
            Country = null,
            DescriptionSnippet = null
        };
}
