using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace UNNYoutubePromoSender;

/// <summary>
/// Отправка писем по SMTP (как во всех почтовых программах). IMAP используется для
/// необязательной копии в папку «Отправленные» — по протоколу IMAP, как у клиента.
/// </summary>
public static class MailKitMailSender
{
    public static async Task SendAsync(
        string smtpHost,
        int smtpPort,
        string smtpEncryption,
        string? imapHost,
        int imapPort,
        string imapEncryption,
        bool copyToSentViaImap,
        string login,
        string password,
        string toAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(smtpHost))
            throw new ArgumentException("Укажите SMTP-сервер.", nameof(smtpHost));
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Укажите логин (email).", nameof(login));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Укажите пароль.", nameof(password));
        if (string.IsNullOrWhiteSpace(toAddress))
            throw new ArgumentException("Укажите получателя.", nameof(toAddress));

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(login));
        message.To.Add(MailboxAddress.Parse(toAddress));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        var smtpSocket = ToSecureSocketOptions(smtpEncryption);

        using (var smtp = new SmtpClient())
        {
            await smtp.ConnectAsync(smtpHost, smtpPort, smtpSocket, cancellationToken).ConfigureAwait(false);
            await smtp.AuthenticateAsync(login, password, cancellationToken).ConfigureAwait(false);
            await smtp.SendAsync(message, cancellationToken).ConfigureAwait(false);
            await smtp.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
        }

        if (!copyToSentViaImap || string.IsNullOrWhiteSpace(imapHost))
            return;

        try
        {
            var imapSocket = ToSecureSocketOptions(imapEncryption);
            using var imap = new ImapClient();
            await imap.ConnectAsync(imapHost, imapPort, imapSocket, cancellationToken).ConfigureAwait(false);
            await imap.AuthenticateAsync(login, password, cancellationToken).ConfigureAwait(false);
            var sent = imap.GetFolder(SpecialFolder.Sent);
            await sent.OpenAsync(FolderAccess.ReadWrite, cancellationToken).ConfigureAwait(false);
            await sent.AppendAsync(message, MessageFlags.Seen, cancellationToken).ConfigureAwait(false);
            await imap.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            // доставка уже выполнена по SMTP; копия в IMAP необязательна
        }
    }

    private static SecureSocketOptions ToSecureSocketOptions(string? mode)
    {
        return mode switch
        {
            "SSL/TLS" => SecureSocketOptions.SslOnConnect,
            "Авто" => SecureSocketOptions.Auto,
            _ => SecureSocketOptions.StartTls
        };
    }
}
