namespace UNNYoutubePromoSender
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            grpYouTube = new GroupBox();
            lblApiKey = new Label();
            txtApiKey = new TextBox();
            lblQuery = new Label();
            txtQuery = new TextBox();
            chkRussianChannelsOnly = new CheckBox();
            chkNonRussiaChannelsOnly = new CheckBox();
            chkSearchFromCacheOnly = new CheckBox();
            lblMinSubs = new Label();
            numMinSubs = new NumericUpDown();
            lblMaxSubs = new Label();
            numMaxSubs = new NumericUpDown();
            lblPages = new Label();
            numSearchPages = new NumericUpDown();
            btnSearch = new Button();
            dgvChannels = new DataGridView();
            btnOpenSeleniumAbout = new Button();
            btnExtractEmail = new Button();
            btnCloseBrowser = new Button();
            chkSkipFilledEmails = new CheckBox();
            btnAutoTableWalk = new Button();
            btnStopTableWalk = new Button();
            grpMailTransport = new GroupBox();
            lblSmtpBlock = new Label();
            txtSmtpHost = new TextBox();
            lblSmtpPortLabel = new Label();
            numSmtpPort = new NumericUpDown();
            lblSmtpEnc = new Label();
            cmbSmtpEncrypt = new ComboBox();
            lblImapBlock = new Label();
            txtImapHost = new TextBox();
            lblImapPortLabel = new Label();
            numImapPort = new NumericUpDown();
            lblImapEnc = new Label();
            cmbImapEncrypt = new ComboBox();
            chkImapCopyToSent = new CheckBox();
            grpMail = new GroupBox();
            lblGmail = new Label();
            txtGmail = new TextBox();
            lblAppPass = new Label();
            txtAppPassword = new TextBox();
            lblTo = new Label();
            txtTo = new TextBox();
            lblSubject = new Label();
            txtSubject = new TextBox();
            lblBody = new Label();
            txtBody = new TextBox();
            btnSendMail = new Button();
            btnBulkSend = new Button();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            grpYouTube.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMinSubs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxSubs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSearchPages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvChannels).BeginInit();
            grpMailTransport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSmtpPort).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numImapPort).BeginInit();
            grpMail.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // grpYouTube
            // 
            grpYouTube.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpYouTube.Controls.Add(lblApiKey);
            grpYouTube.Controls.Add(txtApiKey);
            grpYouTube.Controls.Add(lblQuery);
            grpYouTube.Controls.Add(txtQuery);
            grpYouTube.Controls.Add(chkRussianChannelsOnly);
            grpYouTube.Controls.Add(chkNonRussiaChannelsOnly);
            grpYouTube.Controls.Add(chkSearchFromCacheOnly);
            grpYouTube.Controls.Add(lblMinSubs);
            grpYouTube.Controls.Add(numMinSubs);
            grpYouTube.Controls.Add(lblMaxSubs);
            grpYouTube.Controls.Add(numMaxSubs);
            grpYouTube.Controls.Add(lblPages);
            grpYouTube.Controls.Add(numSearchPages);
            grpYouTube.Controls.Add(btnSearch);
            grpYouTube.Controls.Add(dgvChannels);
            grpYouTube.Controls.Add(btnOpenSeleniumAbout);
            grpYouTube.Controls.Add(btnExtractEmail);
            grpYouTube.Controls.Add(btnCloseBrowser);
            grpYouTube.Controls.Add(chkSkipFilledEmails);
            grpYouTube.Controls.Add(btnAutoTableWalk);
            grpYouTube.Controls.Add(btnStopTableWalk);
            grpYouTube.Location = new Point(12, 12);
            grpYouTube.Name = "grpYouTube";
            grpYouTube.Size = new Size(794, 405);
            grpYouTube.TabIndex = 0;
            grpYouTube.TabStop = false;
            grpYouTube.Text = "YouTube: поиск каналов (API v3)";
            // 
            // lblApiKey
            // 
            lblApiKey.AutoSize = true;
            lblApiKey.Location = new Point(12, 28);
            lblApiKey.Name = "lblApiKey";
            lblApiKey.Size = new Size(60, 15);
            lblApiKey.TabIndex = 0;
            lblApiKey.Text = "API-ключ";
            // 
            // txtApiKey
            // 
            txtApiKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtApiKey.Location = new Point(120, 25);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.Size = new Size(658, 23);
            txtApiKey.TabIndex = 1;
            txtApiKey.UseSystemPasswordChar = true;
            // 
            // lblQuery
            // 
            lblQuery.AutoSize = true;
            lblQuery.Location = new Point(12, 57);
            lblQuery.Name = "lblQuery";
            lblQuery.Size = new Size(99, 15);
            lblQuery.TabIndex = 2;
            lblQuery.Text = "Запрос (необяз.)";
            // 
            // txtQuery
            // 
            txtQuery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtQuery.Location = new Point(120, 54);
            txtQuery.Name = "txtQuery";
            txtQuery.PlaceholderText = "пусто — только диапазон подписчиков (широкий поиск)";
            txtQuery.Size = new Size(658, 23);
            txtQuery.TabIndex = 3;
            // 
            // chkRussianChannelsOnly
            // 
            chkRussianChannelsOnly.AutoSize = true;
            chkRussianChannelsOnly.Location = new Point(12, 81);
            chkRussianChannelsOnly.Name = "chkRussianChannelsOnly";
            chkRussianChannelsOnly.Size = new Size(199, 19);
            chkRussianChannelsOnly.TabIndex = 4;
            chkRussianChannelsOnly.Text = "Только русскоязычные каналы";
            chkRussianChannelsOnly.UseVisualStyleBackColor = true;
            // 
            // chkNonRussiaChannelsOnly
            // 
            chkNonRussiaChannelsOnly.AutoSize = true;
            chkNonRussiaChannelsOnly.Location = new Point(260, 81);
            chkNonRussiaChannelsOnly.Name = "chkNonRussiaChannelsOnly";
            chkNonRussiaChannelsOnly.Size = new Size(177, 19);
            chkNonRussiaChannelsOnly.TabIndex = 18;
            chkNonRussiaChannelsOnly.Text = "Только каналы не в России";
            chkNonRussiaChannelsOnly.UseVisualStyleBackColor = true;
            // 
            // chkSearchFromCacheOnly
            // 
            chkSearchFromCacheOnly.AutoSize = true;
            chkSearchFromCacheOnly.Location = new Point(445, 79);
            chkSearchFromCacheOnly.Name = "chkSearchFromCacheOnly";
            chkSearchFromCacheOnly.Size = new Size(340, 19);
            chkSearchFromCacheOnly.TabIndex = 19;
            chkSearchFromCacheOnly.Text = "Только из кеша на диске (без YouTube API, 0 квоты)";
            chkSearchFromCacheOnly.UseVisualStyleBackColor = true;
            // 
            // lblMinSubs
            // 
            lblMinSubs.AutoSize = true;
            lblMinSubs.Location = new Point(12, 111);
            lblMinSubs.Name = "lblMinSubs";
            lblMinSubs.Size = new Size(97, 15);
            lblMinSubs.TabIndex = 4;
            lblMinSubs.Text = "Подписчиков от";
            // 
            // numMinSubs
            // 
            numMinSubs.Location = new Point(120, 109);
            numMinSubs.Maximum = new decimal(new int[] { 2000000000, 0, 0, 0 });
            numMinSubs.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMinSubs.Name = "numMinSubs";
            numMinSubs.Size = new Size(120, 23);
            numMinSubs.TabIndex = 5;
            numMinSubs.ThousandsSeparator = true;
            numMinSubs.Value = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // lblMaxSubs
            // 
            lblMaxSubs.AutoSize = true;
            lblMaxSubs.Location = new Point(260, 111);
            lblMaxSubs.Name = "lblMaxSubs";
            lblMaxSubs.Size = new Size(20, 15);
            lblMaxSubs.TabIndex = 6;
            lblMaxSubs.Text = "до";
            // 
            // numMaxSubs
            // 
            numMaxSubs.Location = new Point(285, 109);
            numMaxSubs.Maximum = new decimal(new int[] { 2000000000, 0, 0, 0 });
            numMaxSubs.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numMaxSubs.Name = "numMaxSubs";
            numMaxSubs.Size = new Size(120, 23);
            numMaxSubs.TabIndex = 7;
            numMaxSubs.ThousandsSeparator = true;
            numMaxSubs.Value = new decimal(new int[] { 500000, 0, 0, 0 });
            // 
            // lblPages
            // 
            lblPages.AutoSize = true;
            lblPages.Location = new Point(430, 111);
            lblPages.Name = "lblPages";
            lblPages.Size = new Size(96, 15);
            lblPages.TabIndex = 8;
            lblPages.Text = "Страниц поиска";
            // 
            // numSearchPages
            // 
            numSearchPages.Location = new Point(531, 109);
            numSearchPages.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            numSearchPages.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSearchPages.Name = "numSearchPages";
            numSearchPages.Size = new Size(50, 23);
            numSearchPages.TabIndex = 9;
            numSearchPages.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.Location = new Point(619, 106);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(159, 27);
            btnSearch.TabIndex = 10;
            btnSearch.Text = "Найти каналы";
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += BtnSearch_Click;
            // 
            // dgvChannels
            // 
            dgvChannels.AllowUserToAddRows = false;
            dgvChannels.AllowUserToDeleteRows = false;
            dgvChannels.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvChannels.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChannels.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChannels.Location = new Point(12, 138);
            dgvChannels.MultiSelect = false;
            dgvChannels.Name = "dgvChannels";
            dgvChannels.ReadOnly = true;
            dgvChannels.RowHeadersVisible = false;
            dgvChannels.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChannels.Size = new Size(766, 155);
            dgvChannels.TabIndex = 11;
            dgvChannels.DoubleClick += DgvChannels_DoubleClick;
            // 
            // btnOpenSeleniumAbout
            // 
            btnOpenSeleniumAbout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOpenSeleniumAbout.Location = new Point(12, 325);
            btnOpenSeleniumAbout.Name = "btnOpenSeleniumAbout";
            btnOpenSeleniumAbout.Size = new Size(248, 27);
            btnOpenSeleniumAbout.TabIndex = 12;
            btnOpenSeleniumAbout.Text = "Открыть «О канале» и найти email";
            btnOpenSeleniumAbout.UseVisualStyleBackColor = true;
            btnOpenSeleniumAbout.Click += BtnOpenSeleniumAbout_Click;
            // 
            // btnExtractEmail
            // 
            btnExtractEmail.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExtractEmail.Location = new Point(266, 325);
            btnExtractEmail.Name = "btnExtractEmail";
            btnExtractEmail.Size = new Size(178, 27);
            btnExtractEmail.TabIndex = 13;
            btnExtractEmail.Text = "Ещё раз найти email";
            btnExtractEmail.UseVisualStyleBackColor = true;
            btnExtractEmail.Click += BtnExtractEmail_Click;
            // 
            // btnCloseBrowser
            // 
            btnCloseBrowser.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCloseBrowser.Location = new Point(450, 325);
            btnCloseBrowser.Name = "btnCloseBrowser";
            btnCloseBrowser.Size = new Size(130, 27);
            btnCloseBrowser.TabIndex = 14;
            btnCloseBrowser.Text = "Закрыть браузер";
            btnCloseBrowser.UseVisualStyleBackColor = true;
            btnCloseBrowser.Click += BtnCloseBrowser_Click;
            // 
            // chkSkipFilledEmails
            // 
            chkSkipFilledEmails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkSkipFilledEmails.AutoSize = true;
            chkSkipFilledEmails.Checked = true;
            chkSkipFilledEmails.CheckState = CheckState.Checked;
            chkSkipFilledEmails.Location = new Point(12, 300);
            chkSkipFilledEmails.Name = "chkSkipFilledEmails";
            chkSkipFilledEmails.Size = new Size(264, 19);
            chkSkipFilledEmails.TabIndex = 15;
            chkSkipFilledEmails.Text = "Пропускать строки с уже найденным email";
            chkSkipFilledEmails.UseVisualStyleBackColor = true;
            // 
            // btnAutoTableWalk
            // 
            btnAutoTableWalk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAutoTableWalk.Location = new Point(12, 360);
            btnAutoTableWalk.Name = "btnAutoTableWalk";
            btnAutoTableWalk.Size = new Size(220, 27);
            btnAutoTableWalk.TabIndex = 16;
            btnAutoTableWalk.Text = "Авто-обход таблицы";
            btnAutoTableWalk.UseVisualStyleBackColor = true;
            btnAutoTableWalk.Click += BtnAutoTableWalk_Click;
            // 
            // btnStopTableWalk
            // 
            btnStopTableWalk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnStopTableWalk.Enabled = false;
            btnStopTableWalk.Location = new Point(238, 360);
            btnStopTableWalk.Name = "btnStopTableWalk";
            btnStopTableWalk.Size = new Size(90, 27);
            btnStopTableWalk.TabIndex = 17;
            btnStopTableWalk.Text = "Стоп";
            btnStopTableWalk.UseVisualStyleBackColor = true;
            btnStopTableWalk.Click += BtnStopTableWalk_Click;
            // 
            // grpMailTransport
            // 
            grpMailTransport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpMailTransport.Controls.Add(lblSmtpBlock);
            grpMailTransport.Controls.Add(txtSmtpHost);
            grpMailTransport.Controls.Add(lblSmtpPortLabel);
            grpMailTransport.Controls.Add(numSmtpPort);
            grpMailTransport.Controls.Add(lblSmtpEnc);
            grpMailTransport.Controls.Add(cmbSmtpEncrypt);
            grpMailTransport.Controls.Add(lblImapBlock);
            grpMailTransport.Controls.Add(txtImapHost);
            grpMailTransport.Controls.Add(lblImapPortLabel);
            grpMailTransport.Controls.Add(numImapPort);
            grpMailTransport.Controls.Add(lblImapEnc);
            grpMailTransport.Controls.Add(cmbImapEncrypt);
            grpMailTransport.Controls.Add(chkImapCopyToSent);
            grpMailTransport.Location = new Point(12, 414);
            grpMailTransport.Name = "grpMailTransport";
            grpMailTransport.Size = new Size(794, 88);
            grpMailTransport.TabIndex = 3;
            grpMailTransport.TabStop = false;
            grpMailTransport.Text = "Серверы: SMTP (отправка) и IMAP (копия в «Отправленные»)";
            // 
            // lblSmtpBlock
            // 
            lblSmtpBlock.AutoSize = true;
            lblSmtpBlock.Location = new Point(12, 24);
            lblSmtpBlock.Name = "lblSmtpBlock";
            lblSmtpBlock.Size = new Size(37, 15);
            lblSmtpBlock.TabIndex = 0;
            lblSmtpBlock.Text = "SMTP";
            // 
            // txtSmtpHost
            // 
            txtSmtpHost.Location = new Point(60, 21);
            txtSmtpHost.Name = "txtSmtpHost";
            txtSmtpHost.PlaceholderText = "smtp.yandex.ru";
            txtSmtpHost.Size = new Size(264, 23);
            txtSmtpHost.TabIndex = 1;
            // 
            // lblSmtpPortLabel
            // 
            lblSmtpPortLabel.AutoSize = true;
            lblSmtpPortLabel.Location = new Point(330, 24);
            lblSmtpPortLabel.Name = "lblSmtpPortLabel";
            lblSmtpPortLabel.Size = new Size(35, 15);
            lblSmtpPortLabel.TabIndex = 2;
            lblSmtpPortLabel.Text = "Порт";
            // 
            // numSmtpPort
            // 
            numSmtpPort.Location = new Point(368, 22);
            numSmtpPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numSmtpPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSmtpPort.Name = "numSmtpPort";
            numSmtpPort.Size = new Size(56, 23);
            numSmtpPort.TabIndex = 3;
            numSmtpPort.Value = new decimal(new int[] { 587, 0, 0, 0 });
            // 
            // lblSmtpEnc
            // 
            lblSmtpEnc.AutoSize = true;
            lblSmtpEnc.Location = new Point(434, 24);
            lblSmtpEnc.Name = "lblSmtpEnc";
            lblSmtpEnc.Size = new Size(80, 15);
            lblSmtpEnc.TabIndex = 4;
            lblSmtpEnc.Text = "Шифрование";
            // 
            // cmbSmtpEncrypt
            // 
            cmbSmtpEncrypt.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSmtpEncrypt.FormattingEnabled = true;
            cmbSmtpEncrypt.Location = new Point(520, 21);
            cmbSmtpEncrypt.Name = "cmbSmtpEncrypt";
            cmbSmtpEncrypt.Size = new Size(95, 23);
            cmbSmtpEncrypt.TabIndex = 5;
            // 
            // lblImapBlock
            // 
            lblImapBlock.AutoSize = true;
            lblImapBlock.Location = new Point(12, 54);
            lblImapBlock.Name = "lblImapBlock";
            lblImapBlock.Size = new Size(36, 15);
            lblImapBlock.TabIndex = 6;
            lblImapBlock.Text = "IMAP";
            // 
            // txtImapHost
            // 
            txtImapHost.Location = new Point(60, 51);
            txtImapHost.Name = "txtImapHost";
            txtImapHost.PlaceholderText = "imap.yandex.ru";
            txtImapHost.Size = new Size(264, 23);
            txtImapHost.TabIndex = 7;
            // 
            // lblImapPortLabel
            // 
            lblImapPortLabel.AutoSize = true;
            lblImapPortLabel.Location = new Point(330, 54);
            lblImapPortLabel.Name = "lblImapPortLabel";
            lblImapPortLabel.Size = new Size(35, 15);
            lblImapPortLabel.TabIndex = 8;
            lblImapPortLabel.Text = "Порт";
            // 
            // numImapPort
            // 
            numImapPort.Location = new Point(368, 52);
            numImapPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numImapPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numImapPort.Name = "numImapPort";
            numImapPort.Size = new Size(56, 23);
            numImapPort.TabIndex = 9;
            numImapPort.Value = new decimal(new int[] { 993, 0, 0, 0 });
            // 
            // lblImapEnc
            // 
            lblImapEnc.AutoSize = true;
            lblImapEnc.Location = new Point(434, 54);
            lblImapEnc.Name = "lblImapEnc";
            lblImapEnc.Size = new Size(80, 15);
            lblImapEnc.TabIndex = 10;
            lblImapEnc.Text = "Шифрование";
            // 
            // cmbImapEncrypt
            // 
            cmbImapEncrypt.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbImapEncrypt.FormattingEnabled = true;
            cmbImapEncrypt.Location = new Point(520, 51);
            cmbImapEncrypt.Name = "cmbImapEncrypt";
            cmbImapEncrypt.Size = new Size(95, 23);
            cmbImapEncrypt.TabIndex = 11;
            // 
            // chkImapCopyToSent
            // 
            chkImapCopyToSent.AutoSize = true;
            chkImapCopyToSent.Checked = true;
            chkImapCopyToSent.CheckState = CheckState.Checked;
            chkImapCopyToSent.Location = new Point(630, 53);
            chkImapCopyToSent.Name = "chkImapCopyToSent";
            chkImapCopyToSent.Size = new Size(154, 19);
            chkImapCopyToSent.TabIndex = 12;
            chkImapCopyToSent.Text = "Копия в Отправленные";
            chkImapCopyToSent.UseVisualStyleBackColor = true;
            // 
            // grpMail
            // 
            grpMail.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpMail.Controls.Add(lblGmail);
            grpMail.Controls.Add(txtGmail);
            grpMail.Controls.Add(lblAppPass);
            grpMail.Controls.Add(txtAppPassword);
            grpMail.Controls.Add(lblTo);
            grpMail.Controls.Add(txtTo);
            grpMail.Controls.Add(lblSubject);
            grpMail.Controls.Add(txtSubject);
            grpMail.Controls.Add(lblBody);
            grpMail.Controls.Add(txtBody);
            grpMail.Controls.Add(btnSendMail);
            grpMail.Controls.Add(btnBulkSend);
            grpMail.Location = new Point(12, 508);
            grpMail.Name = "grpMail";
            grpMail.Size = new Size(794, 228);
            grpMail.TabIndex = 1;
            grpMail.TabStop = false;
            grpMail.Text = "Почта: текст письма";
            // 
            // lblGmail
            // 
            lblGmail.AutoSize = true;
            lblGmail.Location = new Point(12, 28);
            lblGmail.Name = "lblGmail";
            lblGmail.Size = new Size(96, 15);
            lblGmail.TabIndex = 0;
            lblGmail.Text = "Яндекс (от кого)";
            // 
            // txtGmail
            // 
            txtGmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtGmail.Location = new Point(120, 25);
            txtGmail.Name = "txtGmail";
            txtGmail.PlaceholderText = "you@yandex.ru";
            txtGmail.Size = new Size(658, 23);
            txtGmail.TabIndex = 1;
            // 
            // lblAppPass
            // 
            lblAppPass.AutoSize = true;
            lblAppPass.Location = new Point(12, 57);
            lblAppPass.Name = "lblAppPass";
            lblAppPass.Size = new Size(169, 15);
            lblAppPass.TabIndex = 2;
            lblAppPass.Text = "Пароль приложения Яндекса";
            // 
            // txtAppPassword
            // 
            txtAppPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtAppPassword.Location = new Point(187, 54);
            txtAppPassword.Name = "txtAppPassword";
            txtAppPassword.Size = new Size(591, 23);
            txtAppPassword.TabIndex = 3;
            txtAppPassword.UseSystemPasswordChar = true;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(12, 86);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(76, 15);
            lblTo.TabIndex = 4;
            lblTo.Text = "Кому (email)";
            // 
            // txtTo
            // 
            txtTo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTo.Location = new Point(120, 83);
            txtTo.Name = "txtTo";
            txtTo.PlaceholderText = "для одного письма; массовая — из столбца «Найденный email»";
            txtTo.Size = new Size(658, 23);
            txtTo.TabIndex = 5;
            // 
            // lblSubject
            // 
            lblSubject.AutoSize = true;
            lblSubject.Location = new Point(12, 115);
            lblSubject.Name = "lblSubject";
            lblSubject.Size = new Size(34, 15);
            lblSubject.TabIndex = 6;
            lblSubject.Text = "Тема";
            // 
            // txtSubject
            // 
            txtSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSubject.Location = new Point(120, 112);
            txtSubject.Name = "txtSubject";
            txtSubject.Size = new Size(658, 23);
            txtSubject.TabIndex = 7;
            // 
            // lblBody
            // 
            lblBody.AutoSize = true;
            lblBody.Location = new Point(12, 144);
            lblBody.Name = "lblBody";
            lblBody.Size = new Size(36, 15);
            lblBody.TabIndex = 8;
            lblBody.Text = "Текст";
            // 
            // txtBody
            // 
            txtBody.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtBody.Location = new Point(120, 141);
            txtBody.Multiline = true;
            txtBody.Name = "txtBody";
            txtBody.ScrollBars = ScrollBars.Vertical;
            txtBody.Size = new Size(658, 38);
            txtBody.TabIndex = 9;
            // 
            // btnSendMail
            // 
            btnSendMail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSendMail.Location = new Point(676, 185);
            btnSendMail.Name = "btnSendMail";
            btnSendMail.Size = new Size(102, 27);
            btnSendMail.TabIndex = 10;
            btnSendMail.Text = "Одному";
            btnSendMail.UseVisualStyleBackColor = true;
            btnSendMail.Click += BtnSendMail_Click;
            // 
            // btnBulkSend
            // 
            btnBulkSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBulkSend.Location = new Point(476, 185);
            btnBulkSend.Name = "btnBulkSend";
            btnBulkSend.Size = new Size(194, 27);
            btnBulkSend.TabIndex = 11;
            btnBulkSend.Text = "Рассылка по таблице";
            btnBulkSend.UseVisualStyleBackColor = true;
            btnBulkSend.Click += BtnBulkSend_Click;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Location = new Point(0, 751);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(822, 22);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(45, 17);
            lblStatus.Text = "Готово";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(822, 773);
            Controls.Add(statusStrip);
            Controls.Add(grpMail);
            Controls.Add(grpMailTransport);
            Controls.Add(grpYouTube);
            MinimumSize = new Size(820, 812);
            Name = "Form1";
            Text = "UNN YouTube — поиск каналов и письмо";
            FormClosing += Form1_FormClosing;
            grpYouTube.ResumeLayout(false);
            grpYouTube.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMinSubs).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxSubs).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSearchPages).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvChannels).EndInit();
            grpMailTransport.ResumeLayout(false);
            grpMailTransport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSmtpPort).EndInit();
            ((System.ComponentModel.ISupportInitialize)numImapPort).EndInit();
            grpMail.ResumeLayout(false);
            grpMail.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpYouTube;
        private Label lblApiKey;
        private TextBox txtApiKey;
        private Label lblQuery;
        private TextBox txtQuery;
        private Label lblMinSubs;
        private NumericUpDown numMinSubs;
        private Label lblMaxSubs;
        private NumericUpDown numMaxSubs;
        private Label lblPages;
        private NumericUpDown numSearchPages;
        private Button btnSearch;
        private DataGridView dgvChannels;
        private Button btnOpenSeleniumAbout;
        private Button btnExtractEmail;
        private Button btnCloseBrowser;
        private CheckBox chkSkipFilledEmails;
        private Button btnAutoTableWalk;
        private Button btnStopTableWalk;
        private CheckBox chkRussianChannelsOnly;
        private CheckBox chkNonRussiaChannelsOnly;
        private CheckBox chkSearchFromCacheOnly;
        private GroupBox grpMailTransport;
        private Label lblSmtpBlock;
        private TextBox txtSmtpHost;
        private Label lblSmtpPortLabel;
        private NumericUpDown numSmtpPort;
        private Label lblSmtpEnc;
        private ComboBox cmbSmtpEncrypt;
        private Label lblImapBlock;
        private TextBox txtImapHost;
        private Label lblImapPortLabel;
        private NumericUpDown numImapPort;
        private Label lblImapEnc;
        private ComboBox cmbImapEncrypt;
        private CheckBox chkImapCopyToSent;
        private GroupBox grpMail;
        private Label lblGmail;
        private TextBox txtGmail;
        private Label lblAppPass;
        private TextBox txtAppPassword;
        private Label lblTo;
        private TextBox txtTo;
        private Label lblSubject;
        private TextBox txtSubject;
        private Label lblBody;
        private TextBox txtBody;
        private Button btnSendMail;
        private Button btnBulkSend;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
    }
}

