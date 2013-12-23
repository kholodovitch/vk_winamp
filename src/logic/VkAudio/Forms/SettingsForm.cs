namespace VkAudio.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using VkAudio.Classes;

    public class SettingsForm : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private CheckBox cbAddPrefix;
        private CheckBox cbProxyNeedAuth;
        private CheckBox cbProxyWindowsAuth;
        private CheckBox cbUseIeProxy;
        private CheckBox cbUseProxy;
        private IContainer components;
        private GroupBox gbProxyAuth;
        private GroupBox gbUseProxy;
        private bool IsShown;
        private Label lLoginCaption;
        private Label lPassCaption;
        private Label lProxyAddress;
        private Label lProxyLogin;
        private Label lProxyPass;
        private Label lProxyPort;
        private TextBox tbEmail;
        private TextBox tbPass;
        private TextBox tbProxyAddress;
        private TextBox tbProxyLogin;
        private TextBox tbProxyPass;
        private TextBox tbProxyPort;
        private ToolTip ttMain;

        public SettingsForm()
        {
            MouseEventHandler handler = null;
            this.InitializeComponent();
            Settings settings = Settings.Load();
            if (settings != null)
            {
                this.tbEmail.Text = settings.Email;
                this.tbPass.Text = settings.DecryptPass();
                this.cbAddPrefix.Checked = settings.AddPrefix;
                this.cbUseProxy.Checked = settings.UseProxy;
                this.cbUseIeProxy.Checked = settings.UseIeProxy;
                this.tbProxyAddress.Text = settings.ProxyAddress;
                this.tbProxyPort.Text = settings.ProxyPort.ToString();
                this.cbProxyNeedAuth.Checked = settings.ProxyNeedAuth;
                this.cbProxyWindowsAuth.Checked = settings.ProxyUseWindowsAuth;
                this.tbProxyLogin.Text = settings.ProxyLogin;
                this.tbProxyPass.Text = settings.DecryptProxyPass();
            }
            if (Common.ProcessStreaming)
            {
                this.cbAddPrefix.Enabled = this.cbAddPrefix.Checked = false;
                this.ttMain.SetToolTip(this.cbAddPrefix, "Опция отключена, поддерживается только в Winamp!");
                if (handler == null)
                {
                    handler = delegate (object ls, MouseEventArgs le) {
                        Control childAtPoint = base.GetChildAtPoint(le.Location);
                        if (childAtPoint != null)
                        {
                            if ((childAtPoint == this.cbAddPrefix) && !this.IsShown)
                            {
                                string text = this.ttMain.GetToolTip(this.cbAddPrefix);
                                this.ttMain.Show(text, this.cbAddPrefix, (int) (this.cbAddPrefix.Width / 2), this.cbAddPrefix.Height / 2);
                                this.IsShown = true;
                            }
                        }
                        else
                        {
                            this.ttMain.Hide(this.cbAddPrefix);
                            this.IsShown = false;
                        }
                    };
                }
                base.MouseMove += handler;
            }
            this.UpdateFieldStates();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if ((this.cbUseProxy.Checked && !this.cbUseIeProxy.Checked) && (string.IsNullOrEmpty(this.tbProxyAddress.Text) || string.IsNullOrEmpty(this.tbProxyPort.Text)))
            {
                MessageBox.Show(this, "Требуется ввести адрес и порт прокси-сервера!", "Ошибка");
            }
            else if ((this.cbProxyNeedAuth.Checked && !this.cbProxyWindowsAuth.Checked) && (string.IsNullOrEmpty(this.tbProxyLogin.Text) || string.IsNullOrEmpty(this.tbProxyPass.Text)))
            {
                MessageBox.Show(this, "Требуется ввести логин и пароль для прокси-сервера!", "Ошибка");
            }
            else
            {
                Settings settings = new Settings {
                    Email = this.tbEmail.Text,
                    EncryptedPass = SecurityTools.EncryptString(this.tbPass.Text),
                    AddPrefix = this.cbAddPrefix.Checked,
                    UseProxy = this.cbUseProxy.Checked,
                    UseIeProxy = this.cbUseIeProxy.Checked,
                    ProxyAddress = this.tbProxyAddress.Text
                };
                int result = 80;
                int.TryParse(this.tbProxyPort.Text, out result);
                settings.ProxyPort = result;
                settings.ProxyNeedAuth = this.cbProxyNeedAuth.Checked;
                settings.ProxyUseWindowsAuth = this.cbProxyWindowsAuth.Checked;
                settings.ProxyLogin = this.tbProxyLogin.Text;
                settings.EncryptedProxyPass = SecurityTools.EncryptString(this.tbProxyPass.Text);
                settings.Save();
                base.DialogResult = DialogResult.OK;
                base.Close();
            }
        }

        private void cbProxyNeedAuth_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateFieldStates();
        }

        private void cbProxyWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateFieldStates();
        }

        private void cbUseIeProxy_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateFieldStates();
        }

        private void cbUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateFieldStates();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(SettingsForm));
            this.lLoginCaption = new Label();
            this.lPassCaption = new Label();
            this.tbEmail = new TextBox();
            this.tbPass = new TextBox();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.cbAddPrefix = new CheckBox();
            this.ttMain = new ToolTip(this.components);
            this.gbUseProxy = new GroupBox();
            this.tbProxyAddress = new TextBox();
            this.lProxyAddress = new Label();
            this.cbProxyNeedAuth = new CheckBox();
            this.cbUseIeProxy = new CheckBox();
            this.cbUseProxy = new CheckBox();
            this.lProxyPort = new Label();
            this.tbProxyPass = new TextBox();
            this.lProxyPass = new Label();
            this.tbProxyLogin = new TextBox();
            this.lProxyLogin = new Label();
            this.cbProxyWindowsAuth = new CheckBox();
            this.gbProxyAuth = new GroupBox();
            this.tbProxyPort = new TextBox();
            this.gbUseProxy.SuspendLayout();
            this.gbProxyAuth.SuspendLayout();
            base.SuspendLayout();
            this.lLoginCaption.AutoSize = true;
            this.lLoginCaption.Location = new Point(10, 15);
            this.lLoginCaption.Name = "lLoginCaption";
            this.lLoginCaption.Size = new Size(0x23, 13);
            this.lLoginCaption.TabIndex = 0;
            this.lLoginCaption.Text = "Email:";
            this.lPassCaption.AutoSize = true;
            this.lPassCaption.Location = new Point(10, 40);
            this.lPassCaption.Name = "lPassCaption";
            this.lPassCaption.Size = new Size(0x30, 13);
            this.lPassCaption.TabIndex = 1;
            this.lPassCaption.Text = "Пароль:";
            this.tbEmail.BorderStyle = BorderStyle.FixedSingle;
            this.tbEmail.Location = new Point(0x4a, 12);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new Size(0xd0, 20);
            this.tbEmail.TabIndex = 1;
            this.tbPass.BorderStyle = BorderStyle.FixedSingle;
            this.tbPass.Location = new Point(0x4a, 0x26);
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new Size(0xd0, 20);
            this.tbPass.TabIndex = 2;
            this.btnOk.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnOk.FlatStyle = FlatStyle.Flat;
            this.btnOk.Location = new Point(13, 0x124);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x81, 0x17);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnCancel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Location = new Point(0x99, 0x124);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x81, 0x17);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.cbAddPrefix.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.cbAddPrefix.AutoSize = true;
            this.cbAddPrefix.Checked = true;
            this.cbAddPrefix.CheckState = CheckState.Checked;
            this.cbAddPrefix.FlatStyle = FlatStyle.Flat;
            this.cbAddPrefix.Location = new Point(13, 0x10b);
            this.cbAddPrefix.Name = "cbAddPrefix";
            this.cbAddPrefix.Size = new Size(0xd9, 0x11);
            this.cbAddPrefix.TabIndex = 11;
            this.cbAddPrefix.Text = "Добавлять префикс [ВК] к названиям";
            this.cbAddPrefix.UseVisualStyleBackColor = true;
            this.gbUseProxy.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gbUseProxy.Controls.Add(this.cbProxyNeedAuth);
            this.gbUseProxy.Controls.Add(this.gbProxyAuth);
            this.gbUseProxy.Controls.Add(this.tbProxyPort);
            this.gbUseProxy.Controls.Add(this.lProxyPort);
            this.gbUseProxy.Controls.Add(this.tbProxyAddress);
            this.gbUseProxy.Controls.Add(this.lProxyAddress);
            this.gbUseProxy.Controls.Add(this.cbUseIeProxy);
            this.gbUseProxy.Enabled = false;
            this.gbUseProxy.Location = new Point(13, 0x40);
            this.gbUseProxy.Name = "gbUseProxy";
            this.gbUseProxy.Size = new Size(0x10d, 0xc3);
            this.gbUseProxy.TabIndex = 7;
            this.gbUseProxy.TabStop = false;
            this.tbProxyAddress.BorderStyle = BorderStyle.FixedSingle;
            this.tbProxyAddress.Location = new Point(0x3d, 0x2b);
            this.tbProxyAddress.Name = "tbProxyAddress";
            this.tbProxyAddress.Size = new Size(0xca, 20);
            this.tbProxyAddress.TabIndex = 5;
            this.lProxyAddress.AutoSize = true;
            this.lProxyAddress.Location = new Point(9, 0x2d);
            this.lProxyAddress.Name = "lProxyAddress";
            this.lProxyAddress.Size = new Size(0x29, 13);
            this.lProxyAddress.TabIndex = 3;
            this.lProxyAddress.Text = "Адрес:";
            this.cbProxyNeedAuth.AutoSize = true;
            this.cbProxyNeedAuth.Location = new Point(15, 0x60);
            this.cbProxyNeedAuth.Name = "cbProxyNeedAuth";
            this.cbProxyNeedAuth.Size = new Size(0x93, 0x11);
            this.cbProxyNeedAuth.TabIndex = 7;
            this.cbProxyNeedAuth.Text = "Требуется авторизация";
            this.cbProxyNeedAuth.UseVisualStyleBackColor = true;
            this.cbProxyNeedAuth.CheckedChanged += new EventHandler(this.cbProxyNeedAuth_CheckedChanged);
            this.cbUseIeProxy.AutoSize = true;
            this.cbUseIeProxy.Location = new Point(9, 0x13);
            this.cbUseIeProxy.Name = "cbUseIeProxy";
            this.cbUseIeProxy.Size = new Size(0xeb, 0x11);
            this.cbUseIeProxy.TabIndex = 4;
            this.cbUseIeProxy.Text = "Использовать настройки Internet Explorer";
            this.cbUseIeProxy.UseVisualStyleBackColor = true;
            this.cbUseIeProxy.CheckedChanged += new EventHandler(this.cbUseIeProxy_CheckedChanged);
            this.cbUseProxy.AutoSize = true;
            this.cbUseProxy.Location = new Point(0x16, 0x3f);
            this.cbUseProxy.Name = "cbUseProxy";
            this.cbUseProxy.Size = new Size(0xb1, 0x11);
            this.cbUseProxy.TabIndex = 3;
            this.cbUseProxy.Text = "Использовать прокси-сервер";
            this.cbUseProxy.UseVisualStyleBackColor = true;
            this.cbUseProxy.CheckedChanged += new EventHandler(this.cbUseProxy_CheckedChanged);
            this.lProxyPort.AutoSize = true;
            this.lProxyPort.Location = new Point(9, 0x47);
            this.lProxyPort.Name = "lProxyPort";
            this.lProxyPort.Size = new Size(0x23, 13);
            this.lProxyPort.TabIndex = 5;
            this.lProxyPort.Text = "Порт:";
            this.tbProxyPass.BorderStyle = BorderStyle.FixedSingle;
            this.tbProxyPass.Location = new Point(60, 0x41);
            this.tbProxyPass.Name = "tbProxyPass";
            this.tbProxyPass.PasswordChar = '*';
            this.tbProxyPass.Size = new Size(0xbf, 20);
            this.tbProxyPass.TabIndex = 10;
            this.lProxyPass.AutoSize = true;
            this.lProxyPass.Location = new Point(8, 0x43);
            this.lProxyPass.Name = "lProxyPass";
            this.lProxyPass.Size = new Size(0x30, 13);
            this.lProxyPass.TabIndex = 9;
            this.lProxyPass.Text = "Пароль:";
            this.tbProxyLogin.BorderStyle = BorderStyle.FixedSingle;
            this.tbProxyLogin.Location = new Point(60, 0x27);
            this.tbProxyLogin.Name = "tbProxyLogin";
            this.tbProxyLogin.Size = new Size(0xbf, 20);
            this.tbProxyLogin.TabIndex = 9;
            this.lProxyLogin.AutoSize = true;
            this.lProxyLogin.Location = new Point(8, 0x29);
            this.lProxyLogin.Name = "lProxyLogin";
            this.lProxyLogin.Size = new Size(0x29, 13);
            this.lProxyLogin.TabIndex = 7;
            this.lProxyLogin.Text = "Логин:";
            this.cbProxyWindowsAuth.AutoSize = true;
            this.cbProxyWindowsAuth.Location = new Point(9, 0x13);
            this.cbProxyWindowsAuth.Name = "cbProxyWindowsAuth";
            this.cbProxyWindowsAuth.Size = new Size(0xe4, 0x11);
            this.cbProxyWindowsAuth.TabIndex = 8;
            this.cbProxyWindowsAuth.Text = "Использовать учётную запись Windows";
            this.cbProxyWindowsAuth.UseVisualStyleBackColor = true;
            this.cbProxyWindowsAuth.CheckedChanged += new EventHandler(this.cbProxyWindowsAuth_CheckedChanged);
            this.gbProxyAuth.Controls.Add(this.cbProxyWindowsAuth);
            this.gbProxyAuth.Controls.Add(this.tbProxyLogin);
            this.gbProxyAuth.Controls.Add(this.tbProxyPass);
            this.gbProxyAuth.Controls.Add(this.lProxyLogin);
            this.gbProxyAuth.Controls.Add(this.lProxyPass);
            this.gbProxyAuth.Location = new Point(6, 0x61);
            this.gbProxyAuth.Name = "gbProxyAuth";
            this.gbProxyAuth.Size = new Size(0x101, 0x5c);
            this.gbProxyAuth.TabIndex = 12;
            this.gbProxyAuth.TabStop = false;
            this.tbProxyPort.BorderStyle = BorderStyle.FixedSingle;
            this.tbProxyPort.Location = new Point(0x3d, 0x45);
            this.tbProxyPort.Name = "tbProxyPort";
            this.tbProxyPort.Size = new Size(0x44, 20);
            this.tbProxyPort.TabIndex = 6;
            this.tbProxyPort.KeyPress += new KeyPressEventHandler(this.tbProxyPort_KeyPress);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x124, 0x144);
            base.Controls.Add(this.cbUseProxy);
            base.Controls.Add(this.gbUseProxy);
            base.Controls.Add(this.cbAddPrefix);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.tbPass);
            base.Controls.Add(this.tbEmail);
            base.Controls.Add(this.lPassCaption);
            base.Controls.Add(this.lLoginCaption);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SettingsForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.gbUseProxy.ResumeLayout(false);
            this.gbUseProxy.PerformLayout();
            this.gbProxyAuth.ResumeLayout(false);
            this.gbProxyAuth.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void tbProxyPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void UpdateFieldStates()
        {
            this.tbProxyLogin.Enabled = this.tbProxyPass.Enabled = !this.cbProxyWindowsAuth.Checked;
            this.tbProxyAddress.Enabled = this.tbProxyPort.Enabled = this.cbProxyNeedAuth.Enabled = !this.cbUseIeProxy.Checked;
            this.gbProxyAuth.Enabled = this.cbProxyNeedAuth.Checked && !this.cbUseIeProxy.Checked;
            this.gbUseProxy.Enabled = this.cbUseProxy.Checked;
        }
    }
}

