// Type: VkAudio.Controls.ListViewEx
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Windows.Forms;

namespace VkAudio.Controls
{
  public class ListViewEx : ListView
  {
    private const int WM_HSCROLL = 276;
    private const int WM_VSCROLL = 277;
    private const int MOUSEWHEEL = 522;
    private const int KEYDOWN = 256;
    private bool checkFromDoubleClick;

    public event EventHandler Scroll;

    protected void OnScroll()
    {
      if (this.Scroll == null)
        return;
      this.Scroll((object) this, EventArgs.Empty);
    }

    protected override void WndProc(ref Message m)
    {
      base.WndProc(ref m);
      if (m.Msg != 522 && m.Msg != 277 && (m.Msg != 256 || !(m.WParam == (IntPtr) 40) && !(m.WParam == (IntPtr) 35)))
        return;
      this.OnScroll();
    }

    protected override void OnItemCheck(ItemCheckEventArgs ice)
    {
      if (this.checkFromDoubleClick)
      {
        ice.NewValue = ice.CurrentValue;
        this.checkFromDoubleClick = false;
      }
      else
        base.OnItemCheck(ice);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left && e.Clicks > 1)
        this.checkFromDoubleClick = true;
      base.OnMouseDown(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      this.checkFromDoubleClick = false;
      base.OnKeyDown(e);
    }
  }
}
