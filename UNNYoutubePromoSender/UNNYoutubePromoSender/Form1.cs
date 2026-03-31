using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UNNYoutubePromoSender
{
    public partial class Form1 : Form
    {
        private const int EmailSearchTimeoutSeconds = 8;
        private const int BulkSendDelayMs = 2500;

        private readonly YoutubeChannelSearchService _youtubeSearch = new();
        private IWebDriver? _driver;
        private BindingList<ChannelListItem> _channels = new();
        private CancellationTokenSource? _scanCts;
        private bool _scanRunning;

        public Form1()
        {
            InitializeComponent();
            InitMailTransportCombos();
            dgvChannels.DataSource = _channels;
            ApplyUiSettings(AppSettingsStore.Load());
            LoadSnapshotIntoGridIfAny();
        }

        private void InitMailTransportCombos()
        {
            cmbSmtpEncrypt.Items.AddRange(new object[] { "STARTTLS", "SSL/TLS", "Авто" });
            cmbImapEncrypt.Items.AddRange(new object[] { "SSL/TLS", "STARTTLS", "Авто" });
        }

        private void LoadSnapshotIntoGridIfAny()
        {
            var rows = FoundContactsStore.TryLoadSnapshot();
            if (rows.Count == 0)
                return;
            _channels = new BindingList<ChannelListItem>(rows.ToList());
            dgvChannels.DataSource = _channels;
            SetStatus($"Загружено из сохранения: {_channels.Count} строк");
        }

        private void ApplyUiSettings(AppUiSettings s)
        {
            txtApiKey.Text = s.ApiKey;
            txtQuery.Text = s.SearchQuery;
            SetNumeric(numMinSubs, s.MinSubscribers);
            SetNumeric(numMaxSubs, s.MaxSubscribers);
            SetNumeric(numSearchPages, s.SearchPages);
            chkRussianChannelsOnly.Checked = s.RussianChannelsOnly;
            chkNonRussiaChannelsOnly.Checked = s.NonRussiaChannelsOnly;
            chkSkipFilledEmails.Checked = s.SkipFilledEmails;
            txtGmail.Text = s.GmailAddress;
            txtAppPassword.Text = s.GmailAppPassword;
            txtTo.Text = s.MailTo;
            txtSubject.Text = s.MailSubject;
            txtBody.Text = s.MailBody;

            txtSmtpHost.Text = string.IsNullOrWhiteSpace(s.SmtpHost) ? "smtp.yandex.ru" : s.SmtpHost;
            SetNumeric(numSmtpPort, s.SmtpPort <= 0 ? 587 : s.SmtpPort);
            SelectCombo(cmbSmtpEncrypt, string.IsNullOrWhiteSpace(s.SmtpEncryption) ? "STARTTLS" : s.SmtpEncryption);
            txtImapHost.Text = string.IsNullOrWhiteSpace(s.ImapHost) ? "imap.yandex.ru" : s.ImapHost;
            SetNumeric(numImapPort, s.ImapPort <= 0 ? 993 : s.ImapPort);
            SelectCombo(cmbImapEncrypt, string.IsNullOrWhiteSpace(s.ImapEncryption) ? "SSL/TLS" : s.ImapEncryption);
            chkImapCopyToSent.Checked = s.ImapCopyToSent;
        }

        private static void SelectCombo(ComboBox c, string value)
        {
            if (c.Items.Count == 0)
                return;
            var i = c.Items.IndexOf(value);
            c.SelectedIndex = i >= 0 ? i : 0;
        }

        private static void SetNumeric(NumericUpDown control, decimal value)
        {
            control.Value = Math.Max(control.Minimum, Math.Min(control.Maximum, value));
        }

        private static void SetNumeric(NumericUpDown control, int value) =>
            SetNumeric(control, (decimal)value);

        private void SaveUiSettings()
        {
            try
            {
                AppSettingsStore.Save(new AppUiSettings
                {
                    ApiKey = txtApiKey.Text,
                    SearchQuery = txtQuery.Text,
                    MinSubscribers = numMinSubs.Value,
                    MaxSubscribers = numMaxSubs.Value,
                    SearchPages = (int)numSearchPages.Value,
                    RussianChannelsOnly = chkRussianChannelsOnly.Checked,
                    NonRussiaChannelsOnly = chkNonRussiaChannelsOnly.Checked,
                    SkipFilledEmails = chkSkipFilledEmails.Checked,
                    GmailAddress = txtGmail.Text,
                    GmailAppPassword = txtAppPassword.Text,
                    MailTo = txtTo.Text,
                    MailSubject = txtSubject.Text,
                    MailBody = txtBody.Text,
                    SmtpHost = txtSmtpHost.Text.Trim(),
                    SmtpPort = (int)numSmtpPort.Value,
                    SmtpEncryption = (cmbSmtpEncrypt.SelectedItem as string) ?? "STARTTLS",
                    ImapHost = txtImapHost.Text.Trim(),
                    ImapPort = (int)numImapPort.Value,
                    ImapEncryption = (cmbImapEncrypt.SelectedItem as string) ?? "SSL/TLS",
                    ImapCopyToSent = chkImapCopyToSent.Checked
                });
            }
            catch
            {
                // не мешаем закрытию окна
            }
        }

        private void RunOnUi(Action action)
        {
            try
            {
                if (!IsHandleCreated || IsDisposed)
                    return;
                if (InvokeRequired)
                    Invoke(action);
                else
                    action();
            }
            catch (ObjectDisposedException)
            {
                // форма закрывается
            }
            catch (InvalidOperationException)
            {
                // handle недоступен
            }
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
            statusStrip.Refresh();
        }

        private void CloseDriverQuiet()
        {
            try
            {
                _driver?.Quit();
            }
            catch
            {
                // ignore
            }

            try
            {
                _driver?.Dispose();
            }
            catch
            {
                // ignore
            }

            _driver = null;
        }

        private async void BtnSearch_Click(object? sender, EventArgs e)
        {
            btnSearch.Enabled = false;
            SetStatus("Поиск…");
            _channels = new BindingList<ChannelListItem>();
            dgvChannels.DataSource = _channels;

            try
            {
                var min = (ulong)numMinSubs.Value;
                var max = (ulong)numMaxSubs.Value;
                if (min > max)
                {
                    MessageBox.Show(this, "Минимум подписчиков не может быть больше максимума.", "Проверка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var list = await _youtubeSearch.SearchChannelsAsync(
                    txtApiKey.Text.Trim(),
                    txtQuery.Text,
                    min,
                    max,
                    (int)numSearchPages.Value,
                    chkRussianChannelsOnly.Checked,
                    chkNonRussiaChannelsOnly.Checked,
                    CancellationToken.None).ConfigureAwait(true);

                foreach (var item in list)
                    _channels.Add(item);

                SetStatus($"Найдено каналов: {_channels.Count}");
            }
            catch (Exception ex)
            {
                SetStatus("Ошибка");
                MessageBox.Show(this, ex.Message, "Ошибка поиска", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSearch.Enabled = true;
            }
        }

        private void BtnOpenSeleniumAbout_Click(object? sender, EventArgs e)
        {
            if (dgvChannels.CurrentRow?.DataBoundItem is not ChannelListItem ch)
            {
                MessageBox.Show(this, "Выберите строку в таблице.", "Канал", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            SetBrowserUiBusy(true);
            try
            {
                CloseDriverQuiet();

                var options = new ChromeOptions();
                options.AddArgument("--disable-blink-features=AutomationControlled");
                _driver = new ChromeDriver(options);
                _driver.Navigate().GoToUrl(ch.AboutUrl);
                YoutubeCookieConsent.TryAcceptAll(_driver);
                ApplyExtractedEmail(YoutubeAboutEmailExtractor.TryExtract(_driver, EmailSearchTimeoutSeconds),
                    showDialogOnMiss: true, forRow: ch);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Selenium / Chrome", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBrowserUiBusy(false);
            }
        }

        private void BtnExtractEmail_Click(object? sender, EventArgs e)
        {
            if (_driver == null)
            {
                MessageBox.Show(this, "Сначала откройте страницу «О канале» кнопкой выше.", "Браузер",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ChannelListItem? row = dgvChannels.CurrentRow?.DataBoundItem as ChannelListItem;

            SetBrowserUiBusy(true);
            try
            {
                YoutubeCookieConsent.TryAcceptAll(_driver);
                ApplyExtractedEmail(YoutubeAboutEmailExtractor.TryExtract(_driver, EmailSearchTimeoutSeconds),
                    showDialogOnMiss: true, forRow: row);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Поиск email", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBrowserUiBusy(false);
            }
        }

        private void ApplyExtractedEmail(string? email, bool showDialogOnMiss, ChannelListItem? forRow = null)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var em = email.Trim();
                txtTo.Text = em;
                if (forRow != null)
                {
                    forRow.FoundEmail = em;
                    FoundContactsStore.AppendFound(forRow, em);
                    FoundContactsStore.SaveSnapshot(_channels.ToList());
                }

                SetStatus("Email найден и подставлен в поле «Кому»");
                return;
            }

            SetStatus("Email не найден — при необходимости нажмите «Ещё раз найти email»");
            if (showDialogOnMiss)
            {
                MessageBox.Show(this,
                    "Не удалось автоматически определить адрес. Частые причины: у канала нет публичного email, " +
                    "нужно вручную нажать «Показать адрес» на странице, баннер cookies или смена вёрстки YouTube.",
                    "Контакт на канале",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void SetBrowserUiBusy(bool busy)
        {
            if (_scanRunning)
                return;

            UseWaitCursor = busy;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
            btnOpenSeleniumAbout.Enabled = !busy;
            btnExtractEmail.Enabled = !busy;
            btnSearch.Enabled = !busy;
            btnAutoTableWalk.Enabled = !busy;
            btnBulkSend.Enabled = !busy;
        }

        private void SetScanRunning(bool running)
        {
            _scanRunning = running;
            btnStopTableWalk.Enabled = running;
            btnAutoTableWalk.Enabled = !running;
            btnSearch.Enabled = !running;
            btnOpenSeleniumAbout.Enabled = !running;
            btnExtractEmail.Enabled = !running;
            btnCloseBrowser.Enabled = !running;
            chkSkipFilledEmails.Enabled = !running;
            chkRussianChannelsOnly.Enabled = !running;
            chkNonRussiaChannelsOnly.Enabled = !running;
            btnBulkSend.Enabled = !running;
            UseWaitCursor = running;
            Cursor = running ? Cursors.WaitCursor : Cursors.Default;
        }

        private void BtnAutoTableWalk_Click(object? sender, EventArgs e)
        {
            if (_channels.Count == 0)
            {
                MessageBox.Show(this, "Сначала найдите каналы.", "Таблица пуста", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            _scanCts?.Cancel();
            _scanCts?.Dispose();
            _scanCts = new CancellationTokenSource();
            var token = _scanCts.Token;
            var rows = _channels.ToList();
            var skipFilled = chkSkipFilledEmails.Checked;

            SetScanRunning(true);

            Task.Run(() =>
            {
                ChromeDriver? localDriver = null;
                try
                {
                    RunOnUi(CloseDriverQuiet);

                    var options = new ChromeOptions();
                    options.AddArgument("--disable-blink-features=AutomationControlled");
                    localDriver = new ChromeDriver(options);

                    foreach (var ch in rows)
                    {
                        token.ThrowIfCancellationRequested();

                        if (skipFilled && !string.IsNullOrWhiteSpace(ch.FoundEmail))
                            continue;

                        var title = ch.Title;
                        RunOnUi(() =>
                        {
                            SetStatus($"Авто-обход: {title}");
                            for (var i = 0; i < dgvChannels.Rows.Count; i++)
                            {
                                if (dgvChannels.Rows[i].DataBoundItem == ch)
                                {
                                    dgvChannels.CurrentCell = dgvChannels.Rows[i].Cells[0];
                                    break;
                                }
                            }
                        });

                        localDriver.Navigate().GoToUrl(ch.AboutUrl);
                        YoutubeCookieConsent.TryAcceptAll(localDriver);

                        string? email = null;
                        try
                        {
                            email = YoutubeAboutEmailExtractor.TryExtract(localDriver, EmailSearchTimeoutSeconds, token);
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
                        }
                        catch
                        {
                            // пропускаем сбой на одном канале
                        }

                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            var em = email.Trim();
                            RunOnUi(() =>
                            {
                                ch.FoundEmail = em;
                                txtTo.Text = em;
                            });
                            FoundContactsStore.AppendFound(ch, em);
                            RunOnUi(() => FoundContactsStore.SaveSnapshot(_channels.ToList()));
                        }

                        token.ThrowIfCancellationRequested();
                        Thread.Sleep(450);
                    }

                    RunOnUi(() =>
                    {
                        _driver = localDriver;
                        localDriver = null;
                        SetStatus("Авто-обход завершён");
                        FoundContactsStore.SaveSnapshot(_channels.ToList());
                    });
                }
                catch (OperationCanceledException)
                {
                    RunOnUi(() => SetStatus("Остановлено пользователем"));
                }
                catch (Exception ex)
                {
                    RunOnUi(() =>
                        MessageBox.Show(this, ex.Message, "Авто-обход", MessageBoxButtons.OK, MessageBoxIcon.Error));
                }
                finally
                {
                    if (localDriver != null)
                    {
                        RunOnUi(() => { _driver = localDriver; });
                    }

                    RunOnUi(() => SetScanRunning(false));
                }
            }, token);
        }

        private async Task SendOneMailAsync(string to, string subject, string body, CancellationToken cancellationToken)
        {
            var smtpEnc = (cmbSmtpEncrypt.SelectedItem as string) ?? "STARTTLS";
            var imapEnc = (cmbImapEncrypt.SelectedItem as string) ?? "SSL/TLS";
            await MailKitMailSender.SendAsync(
                txtSmtpHost.Text.Trim(),
                (int)numSmtpPort.Value,
                smtpEnc,
                txtImapHost.Text.Trim(),
                (int)numImapPort.Value,
                imapEnc,
                chkImapCopyToSent.Checked,
                txtGmail.Text.Trim(),
                txtAppPassword.Text,
                to,
                subject,
                body,
                cancellationToken).ConfigureAwait(true);
        }

        private void BtnStopTableWalk_Click(object? sender, EventArgs e)
        {
            _scanCts?.Cancel();
        }

        private void BtnCloseBrowser_Click(object? sender, EventArgs e)
        {
            try
            {
                CloseDriverQuiet();
                SetStatus("Браузер закрыт");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Закрытие браузера", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DgvChannels_DoubleClick(object? sender, EventArgs e)
        {
            if (dgvChannels.CurrentRow?.DataBoundItem is not ChannelListItem ch)
                return;
            try
            {
                Clipboard.SetText(ch.AboutUrl);
                SetStatus("Ссылка «О канале» скопирована в буфер");
            }
            catch
            {
                // ignore clipboard errors
            }
        }

        private async void BtnSendMail_Click(object? sender, EventArgs e)
        {
            btnSendMail.Enabled = false;
            btnBulkSend.Enabled = false;
            SetStatus("Отправка…");
            try
            {
                await SendOneMailAsync(
                        txtTo.Text.Trim(),
                        txtSubject.Text,
                        txtBody.Text,
                        CancellationToken.None)
                    .ConfigureAwait(true);
                SetStatus("Письмо отправлено");
                MessageBox.Show(this, "Письмо отправлено.", "Почта", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SetStatus("Ошибка отправки");
                MessageBox.Show(this, ex.Message, "Ошибка SMTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSendMail.Enabled = true;
                btnBulkSend.Enabled = !_scanRunning;
            }
        }

        private async void BtnBulkSend_Click(object? sender, EventArgs e)
        {
            var withEmail = _channels.Where(c => !string.IsNullOrWhiteSpace(c.FoundEmail)).ToList();
            if (withEmail.Count == 0)
            {
                MessageBox.Show(this, "В таблице нет строк с заполненным столбцом «Найденный email».",
                    "Рассылка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var ok = MessageBox.Show(this,
                $"Будет отправлено писем: {withEmail.Count}.\nТема и текст — как в форме. Пауза между письмами ~2,5 с.\nПродолжить?",
                "Рассылка по таблице",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (ok != DialogResult.Yes)
                return;

            var subject = txtSubject.Text;
            var body = txtBody.Text;

            btnBulkSend.Enabled = false;
            btnSendMail.Enabled = false;
            SetStatus("Массовая рассылка…");

            try
            {
                for (var i = 0; i < withEmail.Count; i++)
                {
                    var ch = withEmail[i];
                    var to = ch.FoundEmail!.Trim();
                    SetStatus($"Рассылка {i + 1}/{withEmail.Count}: {to}");
                    await SendOneMailAsync(to, subject, body, CancellationToken.None).ConfigureAwait(true);
                    if (i < withEmail.Count - 1)
                        await Task.Delay(BulkSendDelayMs, CancellationToken.None).ConfigureAwait(true);
                }

                SetStatus($"Рассылка завершена ({withEmail.Count})");
                MessageBox.Show(this, $"Отправлено писем: {withEmail.Count}.", "Почта",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SetStatus("Ошибка рассылки");
                MessageBox.Show(this, ex.Message, "Ошибка SMTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSendMail.Enabled = true;
                btnBulkSend.Enabled = !_scanRunning;
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                FoundContactsStore.SaveSnapshot(_channels.ToList());
            }
            catch
            {
                // ignore
            }

            SaveUiSettings();
            _scanCts?.Cancel();
            _scanCts?.Dispose();
            _scanCts = null;
            CloseDriverQuiet();
        }
    }
}



