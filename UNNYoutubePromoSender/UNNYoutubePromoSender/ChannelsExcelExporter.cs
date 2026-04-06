using ClosedXML.Excel;

namespace UNNYoutubePromoSender;

public static class ChannelsExcelExporter
{
    public static void ExportToFile(IReadOnlyList<ChannelListItem> channels, string filePath)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Каналы");

        var headers = new[]
        {
            "ID канала",
            "Название",
            "Подписчики",
            "URL канала",
            "Найденный email",
            "Письмо отправлено (UTC)",
            "URL «О канале»"
        };

        for (var c = 0; c < headers.Length; c++)
            ws.Cell(1, c + 1).Value = headers[c];

        ws.Range(1, 1, 1, headers.Length).Style.Font.Bold = true;

        var r = 2;
        foreach (var ch in channels)
        {
            ws.Cell(r, 1).Value = ch.ChannelId;
            ws.Cell(r, 2).Value = ch.Title;
            if (ch.SubscriberCount.HasValue)
                ws.Cell(r, 3).Value = (long)ch.SubscriberCount.Value;
            ws.Cell(r, 4).Value = ch.ChannelUrl;
            ws.Cell(r, 5).Value = ch.FoundEmail ?? "";
            ws.Cell(r, 6).Value = ch.MailSentAtUtc?.ToString("O") ?? "";
            ws.Cell(r, 7).Value = ch.AboutUrl;
            r++;
        }

        ws.Columns().AdjustToContents();
        wb.SaveAs(filePath);
    }
}
