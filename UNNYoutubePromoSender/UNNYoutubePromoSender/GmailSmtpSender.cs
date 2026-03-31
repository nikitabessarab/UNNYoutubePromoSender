using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace UNNYoutubePromoSender;

public static class GmailSmtpSender
{
    public static async Task SendAsync(
        string fromAddress,
        string appPassword,
        string toAddress,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fromAddress))
            throw new ArgumentException("Укажите адрес Gmail.", nameof(fromAddress));
        if (string.IsNullOrWhiteSpace(appPassword))
            throw new ArgumentException("Укажите пароль приложения Google.", nameof(appPassword));
        if (string.IsNullOrWhiteSpace(toAddress))
            throw new ArgumentException("Укажите получателя.", nameof(toAddress));

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(fromAddress));
        message.To.Add(MailboxAddress.Parse(toAddress));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls, cancellationToken)
            .ConfigureAwait(false);
        await client.AuthenticateAsync(fromAddress, appPassword, cancellationToken).ConfigureAwait(false);
        await client.SendAsync(message, cancellationToken).ConfigureAwait(false);
        await client.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
    }
}
