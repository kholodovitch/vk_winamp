namespace VkAudio.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using VkAudio.Classes;

    public class LoadingForm : Form
    {
        private Button btnCancel;
        private IContainer components;
        private static IWin32Window lastOwner = null;
        private Label lStatus;
        private static LoadingForm mainForm = null;
        private ProgressBar pbMain;
        private ToolTip ttMain;
        private static bool waitForNext = false;

        public static  event EventHandler Cancel;

        public LoadingForm()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        internal static void HideBlockingLoading()
        {
            if (!waitForNext)
            {
                if (mainForm != null)
                {
                    mainForm.Close();
                }
                mainForm = null;
            }
            else
            {
                waitForNext = false;
                mainForm.lStatus.Text = "Загрузка...";
            }
        }

        public static void IncrementProgress(int value)
        {
            if (mainForm != null)
            {
                mainForm.pbMain.InvokeOperation((ThreadOperation) (() => (mainForm.pbMain.Value += value)));
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.pbMain = new ProgressBar();
            this.lStatus = new Label();
            this.ttMain = new ToolTip(this.components);
            this.btnCancel = new Button();
            base.SuspendLayout();
            this.pbMain.Dock = DockStyle.Top;
            this.pbMain.Location = new Point(1, 1);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new Size(0x145, 0x17);
            this.pbMain.Style = ProgressBarStyle.Marquee;
            this.pbMain.TabIndex = 0;
            this.lStatus.FlatStyle = FlatStyle.Flat;
            this.lStatus.Location = new Point(4, 0x20);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new Size(0xf1, 13);
            this.lStatus.TabIndex = 1;
            this.lStatus.Text = "Загрузка...";
            this.lStatus.TextAlign = ContentAlignment.MiddleLeft;
            this.btnCancel.FlatStyle = FlatStyle.Popup;
            this.btnCancel.Location = new Point(0xf9, 0x1c);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x147, 0x36);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.lStatus);
            base.Controls.Add(this.pbMain);
            base.FormBorderStyle = FormBorderStyle.None;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "LoadingForm";
            base.Padding = new Padding(1);
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "LoadingForm";
            base.Shown += new EventHandler(this.LoadingForm_Shown);
            base.ResumeLayout(false);
        }

        private void LoadingForm_Shown(object sender, EventArgs e)
        {
            this.ttMain.SetToolTip(this.lStatus, this.lStatus.Text);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Pens.Black, (int) (this.pbMain.Left - 1), (int) (this.pbMain.Top - 1), (int) (this.pbMain.Width + 1), (int) (this.pbMain.Height + 1));
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, base.Width - 1, base.Height - 1);
        }

        internal static void ShowBlockingLoading(IWin32Window Owner, bool WaitForNext, string LoadingText, int? ProgressMaxValue, EventHandler CancelHandler)
        {
            EventHandler handler = null;
            IWin32Window localOwner;
            LoadingForm lf;
            waitForNext = WaitForNext;
            if (mainForm != null)
            {
                if (!string.IsNullOrEmpty(LoadingText))
                {
                    mainForm.lStatus.Text = LoadingText;
                }
            }
            else
            {
                localOwner = null;
                lf = new LoadingForm();
                if ((Owner != null) && (Owner is Form))
                {
                    (Owner as Form).Enabled = false;
                    lf.Owner = Owner as Form;
                    lf.StartPosition = FormStartPosition.Manual;
                    lf.Top = ((Owner as Form).Top + ((Owner as Form).Height / 2)) - (lf.Height / 2);
                    lf.Left = ((Owner as Form).Left + ((Owner as Form).Width / 2)) - (lf.Width / 2);
                    lf.ShowInTaskbar = false;
                    localOwner = Owner;
                }
                else
                {
                    WinAPI.RECT rect;
                    WinAPI.GetWindowRect(Common.PlayerWindowHandle, out rect);
                    lf.StartPosition = FormStartPosition.Manual;
                    lf.Top = (rect.Top + ((rect.Bottom - rect.Top) / 2)) - (lf.Height / 2);
                    lf.Left = (rect.Left + ((rect.Right - rect.Left) / 2)) - (lf.Width / 2);
                    lf.ShowInTaskbar = false;
                    WinAPI.EnableWindow(Common.PlayerWindow.Handle, false);
                    localOwner = Common.PlayerWindow;
                }
                if (ProgressMaxValue.HasValue)
                {
                    lf.pbMain.Style = ProgressBarStyle.Blocks;
                    lf.pbMain.Maximum = ProgressMaxValue.Value;
                    lf.pbMain.Minimum = 0;
                }
                else
                {
                    lf.pbMain.Style = ProgressBarStyle.Marquee;
                }
                lastOwner = Owner;
                if (!string.IsNullOrEmpty(LoadingText))
                {
                    lf.lStatus.Text = LoadingText;
                }
                mainForm = lf;
                if (CancelHandler == null)
                {
                    lf.btnCancel.Visible = false;
                    lf.lStatus.Width += lf.btnCancel.Width + 4;
                }
                else
                {
                    if (handler == null)
                    {
                        handler = delegate (object ls, EventArgs le) {
                            if (Cancel != null)
                            {
                                Cancel(lf, new EventArgs());
                            }
                        };
                    }
                    lf.btnCancel.Click += handler;
                }
                lf.FormClosing += delegate (object ls, FormClosingEventArgs le) {
                    if ((lastOwner != null) && (lastOwner is Form))
                    {
                        (lastOwner as Form).Enabled = true;
                        (lastOwner as Form).Activate();
                    }
                    if (localOwner is NativeWindow)
                    {
                        WinAPI.EnableWindow(Common.PlayerWindowHandle, true);
                        WinAPI.SetForegroundWindow(Common.PlayerWindowHandle);
                    }
                };
                lf.Show(localOwner);
            }
        }

        public static void UpdateStatus(string LoadingText)
        {
            mainForm.lStatus.InvokeOperation(delegate {
                mainForm.lStatus.Text = LoadingText;
                mainForm.ttMain.SetToolTip(mainForm.lStatus, mainForm.lStatus.Text);
            });
        }

        public static bool Existed
        {
            get
            {
                return (mainForm != null);
            }
        }
    }
}

