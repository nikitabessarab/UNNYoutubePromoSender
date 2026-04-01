using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace UNNYoutubePromoSender;

public sealed class ChannelListItem : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private void Notify([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    [Browsable(false)]
    public string ChannelId { get; init; } = "";

    public string Title { get; init; } = "";

    [DisplayName("Подписчики")]
    public ulong? SubscriberCount { get; init; }

    [DisplayName("Канал (URL)")]
    public string ChannelUrl { get; init; } = "";

    private string? _foundEmail;

    [DisplayName("Найденный email")]
    public string? FoundEmail
    {
        get => _foundEmail;
        set
        {
            if (_foundEmail == value)
                return;
            _foundEmail = value;
            Notify();
        }
    }

    private DateTime? _mailSentAtUtc;

    /// <summary>Заполняется только после успешной отправки письма на FoundEmail.</summary>
    [DisplayName("Письмо отправлено (UTC)")]
    public DateTime? MailSentAtUtc
    {
        get => _mailSentAtUtc;
        set
        {
            if (_mailSentAtUtc == value)
                return;
            _mailSentAtUtc = value;
            Notify();
        }
    }

    [Browsable(false)]
    public string AboutUrl => $"https://www.youtube.com/channel/{ChannelId}/about";
}
