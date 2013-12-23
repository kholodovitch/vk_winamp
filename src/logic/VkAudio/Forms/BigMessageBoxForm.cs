// Type: VkAudio.Forms.BigMessageBoxForm
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VkAudio.Forms
{
  public class BigMessageBoxForm : Form
  {
    private IContainer components;
    private Panel panBottom;
    private Button btnClose;
    private TextBox tbMessage;

    private BigMessageBoxForm()
    {
      this.InitializeComponent();
    }

    public BigMessageBoxForm(string caption, string msg)
      : this()
    {
      this.tbMessage.Text = msg;
      this.Text = caption;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager resources = new ComponentResourceManager(typeof (BigMessageBoxForm));
      this.panBottom = new Panel();
      this.btnClose = new Button();
      this.tbMessage = new TextBox();
      this.panBottom.SuspendLayout();
      this.SuspendLayout();
      this.panBottom.Controls.Add((Control) this.btnClose);
      this.panBottom.Dock = DockStyle.Bottom;
      this.panBottom.Location = new Point(0, 337);
      this.panBottom.Name = "panBottom";
      this.panBottom.Size = new Size(341, 42);
      this.panBottom.TabIndex = 1;
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.FlatStyle = FlatStyle.Flat;
      this.btnClose.Location = new Point(241, 10);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(92, 23);
      this.btnClose.TabIndex = 7;
      this.btnClose.Text = "Закрыть";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.tbMessage.BackColor = Color.White;
      this.tbMessage.BorderStyle = BorderStyle.FixedSingle;
      this.tbMessage.Dock = DockStyle.Fill;
      this.tbMessage.Location = new Point(0, 0);
      this.tbMessage.Multiline = true;
      this.tbMessage.Name = "tbMessage";
      this.tbMessage.ReadOnly = true;
      this.tbMessage.ScrollBars = ScrollBars.Vertical;
      this.tbMessage.Size = new Size(341, 337);
      this.tbMessage.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.BackColor = Color.White;
      this.ClientSize = new Size(341, 379);
      this.Controls.Add((Control) this.tbMessage);
      this.Controls.Add((Control) this.panBottom);
	  this.Icon = (Icon)resources.GetObject("$this.Icon");
      this.MinimizeBox = false;
      this.MinimumSize = new Size(349, 406);
      this.Name = "BigMessageBoxForm";
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Сообщение";
      this.panBottom.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
