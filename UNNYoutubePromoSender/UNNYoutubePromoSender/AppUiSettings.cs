namespace UNNYoutubePromoSender;

public sealed class AppUiSettings
{
    public string ApiKey { get; set; } = "";
    public string SearchQuery { get; set; } = "";
    public decimal MinSubscribers { get; set; } = 100_000;
    public decimal MaxSubscribers { get; set; } = 500_000;
    public int SearchPages { get; set; } = 3;
    public bool RussianChannelsOnly { get; set; }
    public bool NonRussiaChannelsOnly { get; set; }
    public bool SkipFilledEmails { get; set; } = true;
    public string GmailAddress { get; set; } = "";
    public string GmailAppPassword { get; set; } = "";
    public string MailTo { get; set; } = "";
    public string MailSubject { get; set; } = "";
    public string MailBody { get; set; } = "";

    /// <summary>SMTP — отправка писем (как в Thunderbird / Outlook).</summary>
    public string SmtpHost { get; set; } = "smtp.yandex.ru";
    public int SmtpPort { get; set; } = 587;
    /// <summary>STARTTLS | SSL/TLS | Авто</summary>
    public string SmtpEncryption { get; set; } = "STARTTLS";

    /// <summary>IMAP — входящая почта; у нас используется для копии в «Отправленные».</summary>
    public string ImapHost { get; set; } = "imap.yandex.ru";
    public int ImapPort { get; set; } = 993;
    /// <summary>SSL/TLS | STARTTLS | Авто</summary>
    public string ImapEncryption { get; set; } = "SSL/TLS";

    public bool ImapCopyToSent { get; set; } = true;
}
