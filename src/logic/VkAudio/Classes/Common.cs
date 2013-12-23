// Type: VkAudio.Classes.Common
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VkAudio.Classes
{
  public static class Common
  {
    private static NativeWindow _PlayerAccessorWindow;
    private static NativeWindow _PlayerWindow;

    public static string WorkFilesDirectory
    {
      get
      {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VkAudio", "Winamp");
      }
    }

    public static string CachePath
    {
      get
      {
        return Path.Combine(Common.WorkFilesDirectory, "cache.xml");
      }
    }

    public static string SongsCachePath
    {
      get
      {
        return Path.Combine(Common.WorkFilesDirectory, "SongsCache/");
      }
    }

    public static IntPtr PlayerHandle { get; set; }

    public static IntPtr PlayerWindowHandle { get; set; }

    public static bool UseUnicode { get; set; }

    public static bool ProcessStreaming { get; set; }

    public static NativeWindow PlayerAccessorWindow
    {
      get
      {
        if (Common._PlayerAccessorWindow == null)
        {
          Common._PlayerAccessorWindow = new NativeWindow();
          Common._PlayerAccessorWindow.AssignHandle(Common.PlayerHandle);
        }
        return Common._PlayerAccessorWindow;
      }
    }

    public static NativeWindow PlayerWindow
    {
      get
      {
        if (Common._PlayerWindow == null)
        {
          Common._PlayerWindow = new NativeWindow();
          Common._PlayerWindow.AssignHandle(Common.PlayerWindowHandle);
        }
        return Common._PlayerWindow;
      }
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    public static IWebProxy GetRequestProxy()
    {
      Settings settings = Settings.Load();
      IWebProxy webProxy = (IWebProxy) null;
      if (settings.UseProxy)
      {
        if (settings.UseIeProxy)
        {
          webProxy = WebRequest.DefaultWebProxy;
        }
        else
        {
          webProxy = (IWebProxy) new WebProxy(settings.ProxyAddress, settings.ProxyPort);
          if (settings.ProxyNeedAuth)
          {
            if (settings.ProxyUseWindowsAuth)
              (webProxy as WebProxy).UseDefaultCredentials = true;
            else
              webProxy.Credentials = (ICredentials) new NetworkCredential(settings.ProxyLogin, settings.DecryptProxyPass());
          }
        }
      }
      return webProxy;
    }
  }
}
