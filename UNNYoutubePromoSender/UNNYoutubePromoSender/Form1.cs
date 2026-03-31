using System.ComponentModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace UNNYoutubePromoSender
{
    public partial class Form1 : Form
    {
        private const int EmailSearchTimeoutSeconds = 8;

        private readonly YoutubeChannelSearchService _youtubeSearch = new();
        private IWebDriver? _driver;
        private BindingList<ChannelListItem> _channels = new();
        private CancellationTokenSource? _scanCts;
        private bool _scanRunning;

        public Form1()
        {
            InitializeComponent();
            dgvChannels.DataSource = _channels;
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
                    txtQuery.Text.Trim(),
                    min,
                    max,
                    (int)numSearchPages.Value,
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
                    showDialogOnMiss: true);
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

            SetBrowserUiBusy(true);
            try
            {
                YoutubeCookieConsent.TryAcceptAll(_driver);
                ApplyExtractedEmail(YoutubeAboutEmailExtractor.TryExtract(_driver, EmailSearchTimeoutSeconds),
                    showDialogOnMiss: true);
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

        private void ApplyExtractedEmail(string? email, bool showDialogOnMiss)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                txtTo.Text = email.Trim();
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
            SetStatus("Отправка…");
            try
            {
                await GmailSmtpSender.SendAsync(
                    txtGmail.Text.Trim(),
                    txtAppPassword.Text,
                    txtTo.Text.Trim(),
                    txtSubject.Text,
                    txtBody.Text,
                    CancellationToken.None).ConfigureAwait(true);
                SetStatus("Письмо отправлено");
                MessageBox.Show(this, "Письмо отправлено.", "Gmail", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SetStatus("Ошибка отправки");
                MessageBox.Show(this, ex.Message, "Ошибка SMTP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSendMail.Enabled = true;
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _scanCts?.Cancel();
            _scanCts?.Dispose();
            _scanCts = null;
            CloseDriverQuiet();
        }
    }
}
