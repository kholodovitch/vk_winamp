// Type: VkAudio.Classes.ControlExtension
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Windows.Forms;

namespace VkAudio.Classes
{
  public static class ControlExtension
  {
    public static void InvokeOperation(this Control TargetControl, ThreadOperation o)
    {
      if (TargetControl.InvokeRequired)
        TargetControl.Invoke((Delegate) new ControlExtension.InvokeOperationDelegate(ControlExtension.InvokeOperation), (object) TargetControl, (object) o);
      else
        o();
    }

    private delegate void InvokeOperationDelegate(Control TargetControl, ThreadOperation o);
  }
}
