using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace UNNYoutubePromoSender;

public sealed class YoutubeChannelSearchService
{
    public async Task<IReadOnlyList<ChannelListItem>> SearchChannelsAsync(
        string apiKey,
        string? searchQuery,
        ulong minSubscribers,
        ulong maxSubscribers,
        int maxSearchPages,
        bool russianChannelsOnly,
        bool nonRussiaChannelsOnly,
        CancellationToken cancellationToken = default)
    {
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
        var result = new List<ChannelListItem>();

        for (var i = 0; i < distinct.Count; i += 50)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var batch = distinct.Skip(i).Take(50).ToList();
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

        return result
            .OrderByDescending(c => c.SubscriberCount)
            .ToList();
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
            var list = new List<string>();
            const string letters = "абвгдежзийклмнопрстуфхцчшщъыьэюя";
            foreach (var c in letters)
                list.Add(c.ToString());
            list.AddRange(new[] { "на", "не", "но", "по", "то", "чт", "как", "для", "это", "где", "все", "топ" });
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
