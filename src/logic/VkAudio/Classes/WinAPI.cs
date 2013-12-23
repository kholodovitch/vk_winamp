// Type: VkAudio.Classes.WinAPI
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Runtime.InteropServices;

namespace VkAudio.Classes
{
  public static class WinAPI
  {
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public extern static bool EnableWindow(IntPtr hWnd, bool bEnable);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
	public extern static bool GetWindowRect(IntPtr hWnd, out WinAPI.RECT lpRect);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
	public extern static bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
	public extern static int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    public struct RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }
  }
}
