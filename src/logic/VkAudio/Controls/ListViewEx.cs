namespace VkAudio.Controls
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    public class ListViewEx : ListView
    {
        private bool checkFromDoubleClick;
        private const int KEYDOWN = 0x100;
        private const int MOUSEWHEEL = 0x20a;
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;

        public event EventHandler Scroll;

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            if (this.checkFromDoubleClick)
            {
                ice.NewValue = ice.CurrentValue;
                this.checkFromDoubleClick = false;
            }
            else
            {
                base.OnItemCheck(ice);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.checkFromDoubleClick = false;
            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (e.Clicks > 1))
            {
                this.checkFromDoubleClick = true;
            }
            base.OnMouseDown(e);
        }

        protected void OnScroll()
        {
            if (this.Scroll != null)
            {
                this.Scroll(this, EventArgs.Empty);
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (((m.Msg == 0x20a) || (m.Msg == 0x115)) || ((m.Msg == 0x100) && ((m.WParam == ((IntPtr) 40)) || (m.WParam == ((IntPtr) 0x23)))))
            {
                this.OnScroll();
            }
        }
    }
}

