// Type: VkAudio.Controls.TrackBarEx
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Windows.Forms;

namespace VkAudio.Controls
{
  public class TrackBarEx : TrackBar
  {
    private int drawingDistance = 13;

    public bool IsSliding { get; private set; }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      this.IsSliding = true;
      base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      this.IsSliding = false;
      if (e.Button != MouseButtons.Left)
        return;
      this.SetValueByMousePos(e);
    }

    protected override void OnValueChanged(EventArgs e)
    {
      base.OnValueChanged(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);
      if (!this.IsSliding)
        return;
      this.IsSliding = false;
      this.OnValueChanged(e);
    }

    private void SetValueByMousePos(MouseEventArgs e)
    {
      if (!this.RectangleToScreen(this.ClientRectangle).Contains(Cursor.Position))
      {
        this.OnValueChanged(new EventArgs());
      }
      else
      {
        if (e.Location.X < this.drawingDistance || e.Location.X > this.ClientSize.Width - this.drawingDistance)
          return;
        this.Value = (int) Math.Round((double) this.Minimum + (double) (this.Maximum - this.Minimum) / (((double) this.ClientSize.Width - (double) this.drawingDistance * 2.0) / (double) (e.Location.X - this.drawingDistance)));
      }
    }

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

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 522)
        return;
      base.WndProc(ref m);
    }
  }
}
