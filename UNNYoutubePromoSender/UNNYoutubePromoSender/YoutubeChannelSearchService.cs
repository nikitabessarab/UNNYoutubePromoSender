using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace UNNYoutubePromoSender;

public sealed class YoutubeChannelSearchService
{
    /// <summary>Останавливаем сбор ID, чтобы не делать десятки вызовов search.list подряд (квота ~100 за вызов).</summary>
    private const int MaxChannelIdsToCollect = 800;

    /// <param name="searchOnlyFromCache">Только локальный кеш, без запросов к YouTube API.</param>
    public async Task<IReadOnlyList<ChannelListItem>> SearchChannelsAsync(
        string apiKey,
        string? searchQuery,
        ulong minSubscribers,
        ulong maxSubscribers,
        int maxSearchPages,
        bool russianChannelsOnly,
        bool nonRussiaChannelsOnly,
        bool searchOnlyFromCache,
        CancellationToken cancellationToken = default)
    {
        if (searchOnlyFromCache)
        {
            await Task.Yield();
            var dict = YoutubeSearchCacheStore.LoadDictionary();
            return FilterFromCache(dict, minSubscribers, maxSubscribers, russianChannelsOnly, nonRussiaChannelsOnly);
        }

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("Укажите API-ключ YouTube Data API v3.", nameof(apiKey));

        var pages = Math.Max(1, maxSearchPages);

        var youtube = new YouTubeService(new BaseClientService.Initializer
        {
            ApiKey = apiKey,
            ApplicationName = "UNNYoutubePromoSender"
        });

        var channelIds = new List<string>();
        var trimmedQuery = searchQuery?.Trim();

        if (!string.IsNullOrEmpty(trimmedQuery))
            await CollectChannelIdsSingleQueryAsync(
                youtube, channelIds, trimmedQuery, pages, russianChannelsOnly, nonRussiaChannelsOnly, cancellationToken)
                .ConfigureAwait(false);
        else
            await CollectChannelIdsBroadAsync(
                youtube, channelIds, pages, russianChannelsOnly, nonRussiaChannelsOnly, cancellationToken)
                .ConfigureAwait(false);

        if (channelIds.Count == 0)
            return Array.Empty<ChannelListItem>();

        var distinct = channelIds.Distinct().ToList();
        var cacheDict = YoutubeSearchCacheStore.LoadDictionary();

        var needFetchIds = new List<string>();
        foreach (var id in distinct)
        {
            if (!cacheDict.ContainsKey(id))
                needFetchIds.Add(id);
        }

        var result = new List<ChannelListItem>();

        foreach (var id in distinct)
        {
            if (!cacheDict.TryGetValue(id, out var cached))
                continue;

            if (cached.SubscriberCount < minSubscribers || cached.SubscriberCount > maxSubscribers)
                continue;

            if (russianChannelsOnly && !cached.PassesRussianChannelHeuristic())
                continue;

            if (nonRussiaChannelsOnly && cached.IsRussiaCountry())
                continue;

            result.Add(cached.ToChannelListItem());
        }

        var cacheModified = false;
        for (var i = 0; i < needFetchIds.Count; i += 50)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var batch = needFetchIds.Skip(i).Take(50).ToList();
            var channelsRequest = youtube.Channels.List("snippet,statistics");
            channelsRequest.Id = string.Join(",", batch);

            var channelsResponse = await channelsRequest.ExecuteAsync(cancellationToken).ConfigureAwait(false);
            if (channelsResponse.Items == null)
                continue;

            foreach (var ch in channelsResponse.Items)
            {
                var id = ch.Id ?? "";
                if (string.IsNullOrEmpty(id))
                    continue;

                var cached = CachedYoutubeChannelFromApi(ch);
                cacheDict[cached.ChannelId] = cached;
                cacheModified = true;

                var subs = ch.Statistics?.SubscriberCount;
                if (subs is null || subs < minSubscribers || subs > maxSubscribers)
                    continue;

                if (russianChannelsOnly && !PassesRussianChannelFilter(ch))
                    continue;

                if (nonRussiaChannelsOnly && IsRussiaChannel(ch))
                    continue;

                var title = ch.Snippet?.Title ?? id;
                var custom = ch.Snippet?.CustomUrl;
                var url = !string.IsNullOrEmpty(custom)
                    ? $"https://www.youtube.com/{custom}"
                    : $"https://www.youtube.com/channel/{id}";

                result.Add(new ChannelListItem
                {
                    ChannelId = id,
                    Title = title,
                    SubscriberCount = subs,
                    ChannelUrl = url
                });
            }
        }

        if (cacheModified)
            YoutubeSearchCacheStore.SaveDictionary(cacheDict);

        return result
            .OrderByDescending(c => c.SubscriberCount)
            .ToList();
    }

    private static IReadOnlyList<ChannelListItem> FilterFromCache(
        Dictionary<string, CachedYoutubeChannel> cache,
        ulong minSubscribers,
        ulong maxSubscribers,
        bool russianChannelsOnly,
        bool nonRussiaChannelsOnly)
    {
        var result = new List<ChannelListItem>();
        foreach (var c in cache.Values)
        {
            if (c.SubscriberCount < minSubscribers || c.SubscriberCount > maxSubscribers)
                continue;
            if (russianChannelsOnly && !c.PassesRussianChannelHeuristic())
                continue;
            if (nonRussiaChannelsOnly && c.IsRussiaCountry())
                continue;
            result.Add(c.ToChannelListItem());
        }

        return result
            .OrderByDescending(c => c.SubscriberCount)
            .ToList();
    }

    private static CachedYoutubeChannel CachedYoutubeChannelFromApi(Channel ch)
    {
        var id = ch.Id ?? "";
        var subs = ch.Statistics?.SubscriberCount ?? 0;
        var title = ch.Snippet?.Title ?? id;
        var custom = ch.Snippet?.CustomUrl;
        var url = !string.IsNullOrEmpty(custom)
            ? $"https://www.youtube.com/{custom}"
            : $"https://www.youtube.com/channel/{id}";
        var desc = ch.Snippet?.Description ?? "";
        if (desc.Length > 500)
            desc = desc[..500];

        return new CachedYoutubeChannel
        {
            ChannelId = id,
            Title = title,
            SubscriberCount = subs,
            ChannelUrl = url,
            DefaultLanguage = ch.Snippet?.DefaultLanguage,
            Country = ch.Snippet?.Country,
            DescriptionSnippet = string.IsNullOrEmpty(desc) ? null : desc
        };
    }

    private static async Task CollectChannelIdsSingleQueryAsync(
        YouTubeService youtube,
        List<string> channelIds,
        string q,
        int maxPages,
        bool russianChannelsOnly,
        bool nonRussiaChannelsOnly,
        CancellationToken cancellationToken)
    {
        string? pageToken = null;
        var pages = 0;

        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            pages++;

            var search = CreateSearchRequest(youtube, q, pageToken, russianChannelsOnly, nonRussiaChannelsOnly);
            var searchResponse = await search.ExecuteAsync(cancellationToken).ConfigureAwait(false);

            AppendChannelIds(channelIds, searchResponse.Items);
            if (channelIds.Count >= MaxChannelIdsToCollect)
                break;
            pageToken = searchResponse.NextPageToken;
        } while (!string.IsNullOrEmpty(pageToken) && pages < maxPages);
    }

    private static async Task CollectChannelIdsBroadAsync(
        YouTubeService youtube,
        List<string> channelIds,
        int maxPages,
        bool russianChannelsOnly,
        bool nonRussiaChannelsOnly,
        CancellationToken cancellationToken)
    {
        var seeds = BroadSeeds(russianChannelsOnly);
        var nextTokenForSeed = new string?[seeds.Count];

        for (var p = 0; p < maxPages; p++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var seedIndex = p % seeds.Count;
            var q = seeds[seedIndex];
            var token = nextTokenForSeed[seedIndex];

            var search = CreateSearchRequest(youtube, q, token, russianChannelsOnly, nonRussiaChannelsOnly);
            var searchResponse = await search.ExecuteAsync(cancellationToken).ConfigureAwait(false);

            AppendChannelIds(channelIds, searchResponse.Items);
            nextTokenForSeed[seedIndex] = searchResponse.NextPageToken;
            if (channelIds.Count >= MaxChannelIdsToCollect)
                break;
        }
    }

    private static SearchResource.ListRequest CreateSearchRequest(
        YouTubeService youtube,
        string q,
        string? pageToken,
        bool russianChannelsOnly,
        bool nonRussiaChannelsOnly)
    {
        var search = youtube.Search.List("snippet");
        search.Q = q;
        search.Type = "channel";
        search.MaxResults = 50;
        search.PageToken = pageToken;

        if (russianChannelsOnly)
        {
            search.RelevanceLanguage = "ru";
            if (!nonRussiaChannelsOnly)
                search.RegionCode = "RU";
        }

        return search;
    }

    private static void AppendChannelIds(ICollection<string> channelIds, IList<SearchResult>? items)
    {
        if (items == null)
            return;

        foreach (var item in items)
        {
            var id = item.Id?.ChannelId;
            if (!string.IsNullOrEmpty(id))
                channelIds.Add(id);
        }
    }

    private static IReadOnlyList<string> BroadSeeds(bool russianChannelsOnly)
    {
        if (russianChannelsOnly)
        {
            // Явные коды Unicode: при поломке кодировки файла строка букв превращалась в "?" и поиск возвращал мусор/пусто.
            var list = new List<string>();
            for (var cp = 0x0430; cp <= 0x044F; cp++)
                list.Add(((char)cp).ToString());
            list.Add(((char)0x0451).ToString());
            list.AddRange(new[]
            {
                "\u043D\u0430", "\u043D\u0435", "\u043D\u043E", "\u043F\u043E", "\u0442\u043E",
                "\u0447\u0442", "\u043A\u0430\u043A", "\u0434\u043B\u044F", "\u044D\u0442\u043E",
                "\u0433\u0434\u0435", "\u0432\u0441\u0435", "\u0442\u043E\u043F"
            });
            return list;
        }

        var en = new List<string>();
        for (var c = 'a'; c <= 'z'; c++)
            en.Add(c.ToString());
        en.AddRange(new[] { "th", "in", "er", "an", "re", "nd", "at", "en", "ti", "es", "or", "te", "of" });
        return en;
    }

    private static bool PassesRussianChannelFilter(Channel ch)
    {
        var lang = ch.Snippet?.DefaultLanguage;
        if (!string.IsNullOrEmpty(lang) && lang.StartsWith("ru", StringComparison.OrdinalIgnoreCase))
            return true;

        if (ContainsCyrillic(ch.Snippet?.Title))
            return true;

        if (ContainsCyrillic(ch.Snippet?.Description))
            return true;

        return false;
    }

    private static bool IsRussiaChannel(Channel ch)
    {
        var country = ch.Snippet?.Country;
        return string.Equals(country, "RU", StringComparison.OrdinalIgnoreCase);
    }

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
}
