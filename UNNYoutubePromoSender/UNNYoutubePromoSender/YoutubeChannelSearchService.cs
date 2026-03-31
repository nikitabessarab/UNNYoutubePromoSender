using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace UNNYoutubePromoSender;

public sealed class YoutubeChannelSearchService
{
    public async Task<IReadOnlyList<ChannelListItem>> SearchChannelsAsync(
        string apiKey,
        string searchQuery,
        ulong minSubscribers,
        ulong maxSubscribers,
        int maxSearchPages,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("Укажите API-ключ YouTube Data API v3.", nameof(apiKey));
        if (string.IsNullOrWhiteSpace(searchQuery))
            throw new ArgumentException("Укажите поисковый запрос.", nameof(searchQuery));

        var youtube = new YouTubeService(new BaseClientService.Initializer
        {
            ApiKey = apiKey,
            ApplicationName = "UNNYoutubePromoSender"
        });

        var channelIds = new List<string>();
        string? pageToken = null;
        var pages = 0;

        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            var search = youtube.Search.List("snippet");
            search.Q = searchQuery;
            search.Type = "channel";
            search.MaxResults = 50;
            search.PageToken = pageToken;

            var searchResponse = await search.ExecuteAsync(cancellationToken).ConfigureAwait(false);
            pages++;

            if (searchResponse.Items != null)
            {
                foreach (var item in searchResponse.Items)
                {
                    var id = item.Id?.ChannelId;
                    if (!string.IsNullOrEmpty(id))
                        channelIds.Add(id);
                }
            }

            pageToken = searchResponse.NextPageToken;
        } while (!string.IsNullOrEmpty(pageToken) && pages < maxSearchPages);

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
}
