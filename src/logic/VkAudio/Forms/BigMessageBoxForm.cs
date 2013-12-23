namespace VkAudio.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class BigMessageBoxForm : Form
    {
        private Button btnClose;
        private IContainer components;
        private Panel panBottom;
        private TextBox tbMessage;

        private BigMessageBoxForm()
        {
            this.InitializeComponent();
        }

        public BigMessageBoxForm(string caption, string msg) : this()
        {
            this.tbMessage.Text = msg;
            this.Text = caption;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(BigMessageBoxForm));
            this.panBottom = new Panel();
            this.btnClose = new Button();
            this.tbMessage = new TextBox();
            this.panBottom.SuspendLayout();
            base.SuspendLayout();
            this.panBottom.Controls.Add(this.btnClose);
            this.panBottom.Dock = DockStyle.Bottom;
            this.panBottom.Location = new Point(0, 0x151);
            this.panBottom.Name = "panBottom";
            this.panBottom.Size = new Size(0x155, 0x2a);
            this.panBottom.TabIndex = 1;
            this.btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Location = new Point(0xf1, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x5c, 0x17);
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
            this.tbMessage.Size = new Size(0x155, 0x151);
            this.tbMessage.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x155, 0x17b);
            base.Controls.Add(this.tbMessage);
            base.Controls.Add(this.panBottom);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x15d, 0x196);
            base.Name = "BigMessageBoxForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Сообщение";
            this.panBottom.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

