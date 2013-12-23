// Type: VkAudio.Forms.LoadingForm
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
  public class LoadingForm : Form
  {
    private static LoadingForm mainForm = (LoadingForm) null;
    private static bool waitForNext = false;
    private static IWin32Window lastOwner = (IWin32Window) null;
    private IContainer components;
    private ProgressBar pbMain;
    private Label lStatus;
    private ToolTip ttMain;
    private Button btnCancel;

    public static bool Existed
    {
      get
      {
        return LoadingForm.mainForm != null;
      }
    }

    public static event EventHandler Cancel;

    static LoadingForm()
    {
    }

    public LoadingForm()
    {
      this.InitializeComponent();
    }

    internal static void ShowBlockingLoading(IWin32Window Owner, bool WaitForNext, string LoadingText, int? ProgressMaxValue, EventHandler CancelHandler)
    {
      LoadingForm.waitForNext = WaitForNext;
      if (LoadingForm.mainForm != null)
      {
        if (string.IsNullOrEmpty(LoadingText))
          return;
        LoadingForm.mainForm.lStatus.Text = LoadingText;
      }
      else
      {
        IWin32Window localOwner = (IWin32Window) null;
        LoadingForm lf = new LoadingForm();
        if (Owner != null && Owner is Form)
        {
          (Owner as Form).Enabled = false;
          lf.Owner = Owner as Form;
          lf.StartPosition = FormStartPosition.Manual;
          lf.Top = (Owner as Form).Top + (Owner as Form).Height / 2 - lf.Height / 2;
          lf.Left = (Owner as Form).Left + (Owner as Form).Width / 2 - lf.Width / 2;
          lf.ShowInTaskbar = false;
          localOwner = Owner;
        }
        else
        {
          WinAPI.RECT lpRect;
          WinAPI.GetWindowRect(Common.PlayerWindowHandle, out lpRect);
          lf.StartPosition = FormStartPosition.Manual;
          lf.Top = lpRect.Top + (lpRect.Bottom - lpRect.Top) / 2 - lf.Height / 2;
          lf.Left = lpRect.Left + (lpRect.Right - lpRect.Left) / 2 - lf.Width / 2;
          lf.ShowInTaskbar = false;
          WinAPI.EnableWindow(Common.PlayerWindow.Handle, false);
          localOwner = (IWin32Window) Common.PlayerWindow;
        }
        if (ProgressMaxValue.HasValue)
        {
          lf.pbMain.Style = ProgressBarStyle.Blocks;
          lf.pbMain.Maximum = ProgressMaxValue.Value;
          lf.pbMain.Minimum = 0;
        }
        else
          lf.pbMain.Style = ProgressBarStyle.Marquee;
        LoadingForm.lastOwner = Owner;
        if (!string.IsNullOrEmpty(LoadingText))
          lf.lStatus.Text = LoadingText;
        LoadingForm.mainForm = lf;
        if (CancelHandler == null)
        {
          lf.btnCancel.Visible = false;
          Label label = lf.lStatus;
          int num = label.Width + (lf.btnCancel.Width + 4);
          label.Width = num;
        }
        else
          lf.btnCancel.Click += (EventHandler) ((ls, le) =>
          {
            if (LoadingForm.Cancel == null)
              return;
            LoadingForm.Cancel((object) lf, new EventArgs());
          });
        lf.FormClosing += (FormClosingEventHandler) ((ls, le) =>
        {
          if (LoadingForm.lastOwner != null && LoadingForm.lastOwner is Form)
          {
            (LoadingForm.lastOwner as Form).Enabled = true;
            (LoadingForm.lastOwner as Form).Activate();
          }
          if (!(localOwner is NativeWindow))
            return;
          WinAPI.EnableWindow(Common.PlayerWindowHandle, true);
          WinAPI.SetForegroundWindow(Common.PlayerWindowHandle);
        });
        lf.Show(localOwner);
      }
    }

    internal static void HideBlockingLoading()
    {
      if (!LoadingForm.waitForNext)
      {
        if (LoadingForm.mainForm != null)
          LoadingForm.mainForm.Close();
        LoadingForm.mainForm = (LoadingForm) null;
      }
      else
      {
        LoadingForm.waitForNext = false;
        LoadingForm.mainForm.lStatus.Text = "Загрузка...";
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      e.Graphics.DrawRectangle(Pens.Black, this.pbMain.Left - 1, this.pbMain.Top - 1, this.pbMain.Width + 1, this.pbMain.Height + 1);
      e.Graphics.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 1);
    }

    public static void IncrementProgress(int value)
    {
      if (LoadingForm.mainForm == null)
        return;
      ControlExtension.InvokeOperation((Control) LoadingForm.mainForm.pbMain, (ThreadOperation) (() => LoadingForm.mainForm.pbMain.Value += value));
    }

    public static void UpdateStatus(string LoadingText)
    {
      ControlExtension.InvokeOperation((Control) LoadingForm.mainForm.lStatus, (ThreadOperation) (() =>
      {
        LoadingForm.mainForm.lStatus.Text = LoadingText;
        LoadingForm.mainForm.ttMain.SetToolTip((Control) LoadingForm.mainForm.lStatus, LoadingForm.mainForm.lStatus.Text);
      }));
    }

    private void LoadingForm_Shown(object sender, EventArgs e)
    {
      this.ttMain.SetToolTip((Control) this.lStatus, this.lStatus.Text);
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
      this.pbMain = new ProgressBar();
      this.lStatus = new Label();
      this.ttMain = new ToolTip(this.components);
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.pbMain.Dock = DockStyle.Top;
      this.pbMain.Location = new Point(1, 1);
      this.pbMain.Name = "pbMain";
      this.pbMain.Size = new Size(325, 23);
      this.pbMain.Style = ProgressBarStyle.Marquee;
      this.pbMain.TabIndex = 0;
      this.lStatus.FlatStyle = FlatStyle.Flat;
      this.lStatus.Location = new Point(4, 32);
      this.lStatus.Name = "lStatus";
      this.lStatus.Size = new Size(241, 13);
      this.lStatus.TabIndex = 1;
      this.lStatus.Text = "Загрузка...";
      this.lStatus.TextAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.FlatStyle = FlatStyle.Popup;
      this.btnCancel.Location = new Point(249, 28);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.BackColor = Color.White;
      this.ClientSize = new Size(327, 54);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lStatus);
      this.Controls.Add((Control) this.pbMain);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "LoadingForm";
      this.Padding = new Padding(1);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "LoadingForm";
      this.Shown += new EventHandler(this.LoadingForm_Shown);
      this.ResumeLayout(false);
    }
  }
}
