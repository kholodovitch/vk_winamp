// Type: VkAudio.Forms.SettingsForm
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VkAudio.Classes;

namespace VkAudio.Forms
{
  public class SettingsForm : Form
  {
    private bool IsShown;
    private IContainer components;
    private Label lLoginCaption;
    private Label lPassCaption;
    private TextBox tbEmail;
    private TextBox tbPass;
    private Button btnOk;
    private Button btnCancel;
    private CheckBox cbAddPrefix;
    private ToolTip ttMain;
    private GroupBox gbUseProxy;
    private CheckBox cbUseProxy;
    private TextBox tbProxyAddress;
    private Label lProxyAddress;
    private CheckBox cbProxyNeedAuth;
    private CheckBox cbUseIeProxy;
    private Label lProxyPort;
    private CheckBox cbProxyWindowsAuth;
    private TextBox tbProxyPass;
    private Label lProxyPass;
    private TextBox tbProxyLogin;
    private Label lProxyLogin;
    private GroupBox gbProxyAuth;
    private TextBox tbProxyPort;

    public SettingsForm()
    {
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
        this.ttMain.SetToolTip((Control) this.cbAddPrefix, "Опция отключена, поддерживается только в Winamp!");
        this.MouseMove += (MouseEventHandler) ((ls, le) =>
        {
          Control local_0 = this.GetChildAtPoint(le.Location);
          if (local_0 != null)
          {
            if (local_0 != this.cbAddPrefix || this.IsShown)
              return;
            this.ttMain.Show(this.ttMain.GetToolTip((Control) this.cbAddPrefix), (IWin32Window) this.cbAddPrefix, this.cbAddPrefix.Width / 2, this.cbAddPrefix.Height / 2);
            this.IsShown = true;
          }
          else
          {
            this.ttMain.Hide((IWin32Window) this.cbAddPrefix);
            this.IsShown = false;
          }
        });
      }
      this.UpdateFieldStates();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (this.cbUseProxy.Checked && !this.cbUseIeProxy.Checked && (string.IsNullOrEmpty(this.tbProxyAddress.Text) || string.IsNullOrEmpty(this.tbProxyPort.Text)))
      {
        int num1 = (int) MessageBox.Show((IWin32Window) this, "Требуется ввести адрес и порт прокси-сервера!", "Ошибка");
      }
      else if (this.cbProxyNeedAuth.Checked && !this.cbProxyWindowsAuth.Checked && (string.IsNullOrEmpty(this.tbProxyLogin.Text) || string.IsNullOrEmpty(this.tbProxyPass.Text)))
      {
        int num2 = (int) MessageBox.Show((IWin32Window) this, "Требуется ввести логин и пароль для прокси-сервера!", "Ошибка");
      }
      else
      {
        Settings settings = new Settings();
        settings.Email = this.tbEmail.Text;
        settings.EncryptedPass = SecurityTools.EncryptString(this.tbPass.Text);
        settings.AddPrefix = this.cbAddPrefix.Checked;
        settings.UseProxy = this.cbUseProxy.Checked;
        settings.UseIeProxy = this.cbUseIeProxy.Checked;
        settings.ProxyAddress = this.tbProxyAddress.Text;
        int result = 80;
        int.TryParse(this.tbProxyPort.Text, out result);
        settings.ProxyPort = result;
        settings.ProxyNeedAuth = this.cbProxyNeedAuth.Checked;
        settings.ProxyUseWindowsAuth = this.cbProxyWindowsAuth.Checked;
        settings.ProxyLogin = this.tbProxyLogin.Text;
        settings.EncryptedProxyPass = SecurityTools.EncryptString(this.tbProxyPass.Text);
        settings.Save();
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void cbUseProxy_CheckedChanged(object sender, EventArgs e)
    {
      this.UpdateFieldStates();
    }

    private void UpdateFieldStates()
    {
      this.tbProxyLogin.Enabled = this.tbProxyPass.Enabled = !this.cbProxyWindowsAuth.Checked;
      this.tbProxyAddress.Enabled = this.tbProxyPort.Enabled = this.cbProxyNeedAuth.Enabled = !this.cbUseIeProxy.Checked;
      this.gbProxyAuth.Enabled = this.cbProxyNeedAuth.Checked && !this.cbUseIeProxy.Checked;
      this.gbUseProxy.Enabled = this.cbUseProxy.Checked;
    }

    private void cbUseIeProxy_CheckedChanged(object sender, EventArgs e)
    {
      this.UpdateFieldStates();
    }

    private void cbProxyNeedAuth_CheckedChanged(object sender, EventArgs e)
    {
      this.UpdateFieldStates();
    }

    private void cbProxyWindowsAuth_CheckedChanged(object sender, EventArgs e)
    {
      this.UpdateFieldStates();
    }

    private void tbProxyPort_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar))
        return;
      e.Handled = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SettingsForm));
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
      this.SuspendLayout();
      this.lLoginCaption.AutoSize = true;
      this.lLoginCaption.Location = new Point(10, 15);
      this.lLoginCaption.Name = "lLoginCaption";
      this.lLoginCaption.Size = new Size(35, 13);
      this.lLoginCaption.TabIndex = 0;
      this.lLoginCaption.Text = "Email:";
      this.lPassCaption.AutoSize = true;
      this.lPassCaption.Location = new Point(10, 40);
      this.lPassCaption.Name = "lPassCaption";
      this.lPassCaption.Size = new Size(48, 13);
      this.lPassCaption.TabIndex = 1;
      this.lPassCaption.Text = "Пароль:";
      this.tbEmail.BorderStyle = BorderStyle.FixedSingle;
      this.tbEmail.Location = new Point(74, 12);
      this.tbEmail.Name = "tbEmail";
      this.tbEmail.Size = new Size(208, 20);
      this.tbEmail.TabIndex = 1;
      this.tbPass.BorderStyle = BorderStyle.FixedSingle;
      this.tbPass.Location = new Point(74, 38);
      this.tbPass.Name = "tbPass";
      this.tbPass.PasswordChar = '*';
      this.tbPass.Size = new Size(208, 20);
      this.tbPass.TabIndex = 2;
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnOk.FlatStyle = FlatStyle.Flat;
      this.btnOk.Location = new Point(13, 292);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(129, 23);
      this.btnOk.TabIndex = 12;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnCancel.FlatStyle = FlatStyle.Flat;
      this.btnCancel.Location = new Point(153, 292);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(129, 23);
      this.btnCancel.TabIndex = 13;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.cbAddPrefix.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cbAddPrefix.AutoSize = true;
      this.cbAddPrefix.Checked = true;
      this.cbAddPrefix.CheckState = CheckState.Checked;
      this.cbAddPrefix.FlatStyle = FlatStyle.Flat;
      this.cbAddPrefix.Location = new Point(13, 267);
      this.cbAddPrefix.Name = "cbAddPrefix";
      this.cbAddPrefix.Size = new Size(217, 17);
      this.cbAddPrefix.TabIndex = 11;
      this.cbAddPrefix.Text = "Добавлять префикс [ВК] к названиям";
      this.cbAddPrefix.UseVisualStyleBackColor = true;
      this.gbUseProxy.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gbUseProxy.Controls.Add((Control) this.cbProxyNeedAuth);
      this.gbUseProxy.Controls.Add((Control) this.gbProxyAuth);
      this.gbUseProxy.Controls.Add((Control) this.tbProxyPort);
      this.gbUseProxy.Controls.Add((Control) this.lProxyPort);
      this.gbUseProxy.Controls.Add((Control) this.tbProxyAddress);
      this.gbUseProxy.Controls.Add((Control) this.lProxyAddress);
      this.gbUseProxy.Controls.Add((Control) this.cbUseIeProxy);
      this.gbUseProxy.Enabled = false;
      this.gbUseProxy.Location = new Point(13, 64);
      this.gbUseProxy.Name = "gbUseProxy";
      this.gbUseProxy.Size = new Size(269, 195);
      this.gbUseProxy.TabIndex = 7;
      this.gbUseProxy.TabStop = false;
      this.tbProxyAddress.BorderStyle = BorderStyle.FixedSingle;
      this.tbProxyAddress.Location = new Point(61, 43);
      this.tbProxyAddress.Name = "tbProxyAddress";
      this.tbProxyAddress.Size = new Size(202, 20);
      this.tbProxyAddress.TabIndex = 5;
      this.lProxyAddress.AutoSize = true;
      this.lProxyAddress.Location = new Point(9, 45);
      this.lProxyAddress.Name = "lProxyAddress";
      this.lProxyAddress.Size = new Size(41, 13);
      this.lProxyAddress.TabIndex = 3;
      this.lProxyAddress.Text = "Адрес:";
      this.cbProxyNeedAuth.AutoSize = true;
      this.cbProxyNeedAuth.Location = new Point(15, 96);
      this.cbProxyNeedAuth.Name = "cbProxyNeedAuth";
      this.cbProxyNeedAuth.Size = new Size(147, 17);
      this.cbProxyNeedAuth.TabIndex = 7;
      this.cbProxyNeedAuth.Text = "Требуется авторизация";
      this.cbProxyNeedAuth.UseVisualStyleBackColor = true;
      this.cbProxyNeedAuth.CheckedChanged += new EventHandler(this.cbProxyNeedAuth_CheckedChanged);
      this.cbUseIeProxy.AutoSize = true;
      this.cbUseIeProxy.Location = new Point(9, 19);
      this.cbUseIeProxy.Name = "cbUseIeProxy";
      this.cbUseIeProxy.Size = new Size(235, 17);
      this.cbUseIeProxy.TabIndex = 4;
      this.cbUseIeProxy.Text = "Использовать настройки Internet Explorer";
      this.cbUseIeProxy.UseVisualStyleBackColor = true;
      this.cbUseIeProxy.CheckedChanged += new EventHandler(this.cbUseIeProxy_CheckedChanged);
      this.cbUseProxy.AutoSize = true;
      this.cbUseProxy.Location = new Point(22, 63);
      this.cbUseProxy.Name = "cbUseProxy";
      this.cbUseProxy.Size = new Size(177, 17);
      this.cbUseProxy.TabIndex = 3;
      this.cbUseProxy.Text = "Использовать прокси-сервер";
      this.cbUseProxy.UseVisualStyleBackColor = true;
      this.cbUseProxy.CheckedChanged += new EventHandler(this.cbUseProxy_CheckedChanged);
      this.lProxyPort.AutoSize = true;
      this.lProxyPort.Location = new Point(9, 71);
      this.lProxyPort.Name = "lProxyPort";
      this.lProxyPort.Size = new Size(35, 13);
      this.lProxyPort.TabIndex = 5;
      this.lProxyPort.Text = "Порт:";
      this.tbProxyPass.BorderStyle = BorderStyle.FixedSingle;
      this.tbProxyPass.Location = new Point(60, 65);
      this.tbProxyPass.Name = "tbProxyPass";
      this.tbProxyPass.PasswordChar = '*';
      this.tbProxyPass.Size = new Size(191, 20);
      this.tbProxyPass.TabIndex = 10;
      this.lProxyPass.AutoSize = true;
      this.lProxyPass.Location = new Point(8, 67);
      this.lProxyPass.Name = "lProxyPass";
      this.lProxyPass.Size = new Size(48, 13);
      this.lProxyPass.TabIndex = 9;
      this.lProxyPass.Text = "Пароль:";
      this.tbProxyLogin.BorderStyle = BorderStyle.FixedSingle;
      this.tbProxyLogin.Location = new Point(60, 39);
      this.tbProxyLogin.Name = "tbProxyLogin";
      this.tbProxyLogin.Size = new Size(191, 20);
      this.tbProxyLogin.TabIndex = 9;
      this.lProxyLogin.AutoSize = true;
      this.lProxyLogin.Location = new Point(8, 41);
      this.lProxyLogin.Name = "lProxyLogin";
      this.lProxyLogin.Size = new Size(41, 13);
      this.lProxyLogin.TabIndex = 7;
      this.lProxyLogin.Text = "Логин:";
      this.cbProxyWindowsAuth.AutoSize = true;
      this.cbProxyWindowsAuth.Location = new Point(9, 19);
      this.cbProxyWindowsAuth.Name = "cbProxyWindowsAuth";
      this.cbProxyWindowsAuth.Size = new Size(228, 17);
      this.cbProxyWindowsAuth.TabIndex = 8;
      this.cbProxyWindowsAuth.Text = "Использовать учётную запись Windows";
      this.cbProxyWindowsAuth.UseVisualStyleBackColor = true;
      this.cbProxyWindowsAuth.CheckedChanged += new EventHandler(this.cbProxyWindowsAuth_CheckedChanged);
      this.gbProxyAuth.Controls.Add((Control) this.cbProxyWindowsAuth);
      this.gbProxyAuth.Controls.Add((Control) this.tbProxyLogin);
      this.gbProxyAuth.Controls.Add((Control) this.tbProxyPass);
      this.gbProxyAuth.Controls.Add((Control) this.lProxyLogin);
      this.gbProxyAuth.Controls.Add((Control) this.lProxyPass);
      this.gbProxyAuth.Location = new Point(6, 97);
      this.gbProxyAuth.Name = "gbProxyAuth";
      this.gbProxyAuth.Size = new Size(257, 92);
      this.gbProxyAuth.TabIndex = 12;
      this.gbProxyAuth.TabStop = false;
      this.tbProxyPort.BorderStyle = BorderStyle.FixedSingle;
      this.tbProxyPort.Location = new Point(61, 69);
      this.tbProxyPort.Name = "tbProxyPort";
      this.tbProxyPort.Size = new Size(68, 20);
      this.tbProxyPort.TabIndex = 6;
      this.tbProxyPort.KeyPress += new KeyPressEventHandler(this.tbProxyPort_KeyPress);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(292, 324);
      this.Controls.Add((Control) this.cbUseProxy);
      this.Controls.Add((Control) this.gbUseProxy);
      this.Controls.Add((Control) this.cbAddPrefix);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.tbPass);
      this.Controls.Add((Control) this.tbEmail);
      this.Controls.Add((Control) this.lPassCaption);
      this.Controls.Add((Control) this.lLoginCaption);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SettingsForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Настройки";
      this.gbUseProxy.ResumeLayout(false);
      this.gbUseProxy.PerformLayout();
      this.gbProxyAuth.ResumeLayout(false);
      this.gbProxyAuth.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
