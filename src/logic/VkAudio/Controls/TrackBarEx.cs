namespace VkAudio.Controls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class TrackBarEx : TrackBar
    {
        private int drawingDistance = 13;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    e.SuppressKeyPress = true;
                    break;
            }
            base.OnKeyDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.IsSliding = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (this.IsSliding)
            {
                this.IsSliding = false;
                this.OnValueChanged(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.IsSliding = false;
            if (e.Button == MouseButtons.Left)
            {
                this.SetValueByMousePos(e);
            }
        }

        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e);
        }

        private void SetValueByMousePos(MouseEventArgs e)
        {
            if (!base.RectangleToScreen(base.ClientRectangle).Contains(Cursor.Position))
            {
                this.OnValueChanged(new EventArgs());
            }
            else if ((e.Location.X >= this.drawingDistance) && (e.Location.X <= (base.ClientSize.Width - this.drawingDistance)))
            {
                int num = e.Location.X - this.drawingDistance;
                double num2 = (base.ClientSize.Width - (this.drawingDistance * 2.0)) / ((double) num);
                double minimum = base.Minimum;
                double num4 = base.Maximum - base.Minimum;
                double a = minimum + (num4 / num2);
                base.Value = (int) Math.Round(a);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg != 0x20a)
            {
                base.WndProc(ref m);
            }
        }

        public bool IsSliding { get; private set; }
    }
}

