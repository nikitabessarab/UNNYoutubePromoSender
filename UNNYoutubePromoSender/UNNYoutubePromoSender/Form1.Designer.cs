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
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            grpYouTube.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMinSubs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMaxSubs).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSearchPages).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvChannels).BeginInit();
            grpMail.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            //
            // grpYouTube
            //
            grpYouTube.Controls.Add(lblApiKey);
            grpYouTube.Controls.Add(txtApiKey);
            grpYouTube.Controls.Add(lblQuery);
            grpYouTube.Controls.Add(txtQuery);
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
            grpYouTube.Size = new Size(776, 400);
            grpYouTube.TabIndex = 0;
            grpYouTube.TabStop = false;
            grpYouTube.Text = "YouTube: поиск каналов (API v3)";
            //
            // lblApiKey
            //
            lblApiKey.AutoSize = true;
            lblApiKey.Location = new Point(12, 28);
            lblApiKey.Name = "lblApiKey";
            lblApiKey.Size = new Size(51, 15);
            lblApiKey.TabIndex = 0;
            lblApiKey.Text = "API-ключ";
            //
            // txtApiKey
            //
            txtApiKey.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtApiKey.Location = new Point(120, 25);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.Size = new Size(640, 23);
            txtApiKey.TabIndex = 1;
            txtApiKey.UseSystemPasswordChar = true;
            //
            // lblQuery
            //
            lblQuery.AutoSize = true;
            lblQuery.Location = new Point(12, 57);
            lblQuery.Name = "lblQuery";
            lblQuery.Size = new Size(42, 15);
            lblQuery.TabIndex = 2;
            lblQuery.Text = "Запрос";
            //
            // txtQuery
            //
            txtQuery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtQuery.Location = new Point(120, 54);
            txtQuery.Name = "txtQuery";
            txtQuery.PlaceholderText = "например: программирование обучение";
            txtQuery.Size = new Size(640, 23);
            txtQuery.TabIndex = 3;
            //
            // lblMinSubs
            //
            lblMinSubs.AutoSize = true;
            lblMinSubs.Location = new Point(12, 86);
            lblMinSubs.Name = "lblMinSubs";
            lblMinSubs.Size = new Size(102, 15);
            lblMinSubs.TabIndex = 4;
            lblMinSubs.Text = "Подписчиков от";
            //
            // numMinSubs
            //
            numMinSubs.Location = new Point(120, 84);
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
            lblMaxSubs.Location = new Point(260, 86);
            lblMaxSubs.Name = "lblMaxSubs";
            lblMaxSubs.Size = new Size(19, 15);
            lblMaxSubs.TabIndex = 6;
            lblMaxSubs.Text = "до";
            //
            // numMaxSubs
            //
            numMaxSubs.Location = new Point(285, 84);
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
            lblPages.Location = new Point(430, 86);
            lblPages.Name = "lblPages";
            lblPages.Size = new Size(95, 15);
            lblPages.TabIndex = 8;
            lblPages.Text = "Страниц поиска";
            //
            // numSearchPages
            //
            numSearchPages.Location = new Point(531, 84);
            numSearchPages.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numSearchPages.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSearchPages.Name = "numSearchPages";
            numSearchPages.Size = new Size(50, 23);
            numSearchPages.TabIndex = 9;
            numSearchPages.Value = new decimal(new int[] { 3, 0, 0, 0 });
            //
            // btnSearch
            //
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.Location = new Point(601, 82);
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
            dgvChannels.Location = new Point(12, 115);
            dgvChannels.MultiSelect = false;
            dgvChannels.Name = "dgvChannels";
            dgvChannels.ReadOnly = true;
            dgvChannels.RowHeadersVisible = false;
            dgvChannels.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChannels.Size = new Size(748, 178);
            dgvChannels.TabIndex = 11;
            dgvChannels.DoubleClick += DgvChannels_DoubleClick;
            //
            // btnOpenSeleniumAbout
            //
            btnOpenSeleniumAbout.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOpenSeleniumAbout.Location = new Point(12, 323);
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
            btnExtractEmail.Location = new Point(266, 323);
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
            btnCloseBrowser.Location = new Point(450, 323);
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
            chkSkipFilledEmails.Location = new Point(12, 298);
            chkSkipFilledEmails.Name = "chkSkipFilledEmails";
            chkSkipFilledEmails.Size = new Size(320, 19);
            chkSkipFilledEmails.TabIndex = 15;
            chkSkipFilledEmails.Text = "Пропускать строки с уже найденным email";
            chkSkipFilledEmails.UseVisualStyleBackColor = true;
            chkSkipFilledEmails.Checked = true;
            //
            // btnAutoTableWalk
            //
            btnAutoTableWalk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAutoTableWalk.Location = new Point(12, 358);
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
            btnStopTableWalk.Location = new Point(238, 358);
            btnStopTableWalk.Name = "btnStopTableWalk";
            btnStopTableWalk.Size = new Size(90, 27);
            btnStopTableWalk.TabIndex = 17;
            btnStopTableWalk.Text = "Стоп";
            btnStopTableWalk.UseVisualStyleBackColor = true;
            btnStopTableWalk.Click += BtnStopTableWalk_Click;
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
            grpMail.Location = new Point(12, 418);
            grpMail.Name = "grpMail";
            grpMail.Size = new Size(776, 200);
            grpMail.TabIndex = 1;
            grpMail.TabStop = false;
            grpMail.Text = "Письмо (Gmail SMTP, пароль приложения)";
            //
            // lblGmail
            //
            lblGmail.AutoSize = true;
            lblGmail.Location = new Point(12, 28);
            lblGmail.Name = "lblGmail";
            lblGmail.Size = new Size(96, 15);
            lblGmail.TabIndex = 0;
            lblGmail.Text = "Ваш Gmail (от)";
            //
            // txtGmail
            //
            txtGmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtGmail.Location = new Point(120, 25);
            txtGmail.Name = "txtGmail";
            txtGmail.PlaceholderText = "you@gmail.com";
            txtGmail.Size = new Size(640, 23);
            txtGmail.TabIndex = 1;
            //
            // lblAppPass
            //
            lblAppPass.AutoSize = true;
            lblAppPass.Location = new Point(12, 57);
            lblAppPass.Name = "lblAppPass";
            lblAppPass.Size = new Size(107, 15);
            lblAppPass.TabIndex = 2;
            lblAppPass.Text = "Пароль приложения";
            //
            // txtAppPassword
            //
            txtAppPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtAppPassword.Location = new Point(120, 54);
            txtAppPassword.Name = "txtAppPassword";
            txtAppPassword.Size = new Size(640, 23);
            txtAppPassword.TabIndex = 3;
            txtAppPassword.UseSystemPasswordChar = true;
            //
            // lblTo
            //
            lblTo.AutoSize = true;
            lblTo.Location = new Point(12, 86);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(75, 15);
            lblTo.TabIndex = 4;
            lblTo.Text = "Кому (email)";
            //
            // txtTo
            //
            txtTo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTo.Location = new Point(120, 83);
            txtTo.Name = "txtTo";
            txtTo.PlaceholderText = "подставится после поиска на «О канале» или вставьте вручную";
            txtTo.Size = new Size(640, 23);
            txtTo.TabIndex = 5;
            //
            // lblSubject
            //
            lblSubject.AutoSize = true;
            lblSubject.Location = new Point(12, 115);
            lblSubject.Name = "lblSubject";
            lblSubject.Size = new Size(58, 15);
            lblSubject.TabIndex = 6;
            lblSubject.Text = "Тема";
            //
            // txtSubject
            //
            txtSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSubject.Location = new Point(120, 112);
            txtSubject.Name = "txtSubject";
            txtSubject.Size = new Size(640, 23);
            txtSubject.TabIndex = 7;
            //
            // lblBody
            //
            lblBody.AutoSize = true;
            lblBody.Location = new Point(12, 144);
            lblBody.Name = "lblBody";
            lblBody.Size = new Size(37, 15);
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
            txtBody.Size = new Size(532, 47);
            txtBody.TabIndex = 9;
            //
            // btnSendMail
            //
            btnSendMail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSendMail.Location = new Point(658, 161);
            btnSendMail.Name = "btnSendMail";
            btnSendMail.Size = new Size(102, 27);
            btnSendMail.TabIndex = 10;
            btnSendMail.Text = "Отправить";
            btnSendMail.UseVisualStyleBackColor = true;
            btnSendMail.Click += BtnSendMail_Click;
            //
            // statusStrip
            //
            statusStrip.Dock = DockStyle.Bottom;
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(800, 22);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            //
            // lblStatus
            //
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(118, 17);
            lblStatus.Text = "Готово";
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 650);
            Controls.Add(statusStrip);
            Controls.Add(grpMail);
            Controls.Add(grpYouTube);
            MinimumSize = new Size(820, 680);
            Name = "Form1";
            Text = "UNN YouTube — поиск каналов и письмо";
            FormClosing += Form1_FormClosing;
            grpYouTube.ResumeLayout(false);
            grpYouTube.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMinSubs).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMaxSubs).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSearchPages).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvChannels).EndInit();
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
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
    }
}
