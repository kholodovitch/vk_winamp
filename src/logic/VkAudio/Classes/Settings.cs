// Type: VkAudio.Classes.Settings
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System.IO;

namespace VkAudio.Classes
{
  public class Settings
  {
    public string Email { get; set; }

    public byte[] EncryptedPass { get; set; }

    public bool AddPrefix { get; set; }

    public bool DownloadImmediate { get; set; }

    public bool UseProxy { get; set; }

    public bool UseIeProxy { get; set; }

    public string ProxyAddress { get; set; }

    public int ProxyPort { get; set; }

    public bool ProxyNeedAuth { get; set; }

    public bool ProxyUseWindowsAuth { get; set; }

    public string ProxyLogin { get; set; }

    public byte[] EncryptedProxyPass { get; set; }

    public static Settings Load()
    {
      return Serialization.Deserialize<Settings>(Settings.GetPath());
    }

    public void Save()
    {
      Serialization.Serialize((object) this, Settings.GetPath());
    }

    private static string GetPath()
    {
      return Path.Combine(Common.WorkFilesDirectory, "settings.xml");
    }

    public string DecryptPass()
    {
      return SecurityTools.DecryptString(this.EncryptedPass);
    }

    public string DecryptProxyPass()
    {
      return SecurityTools.DecryptString(this.EncryptedProxyPass);
    }
  }
}
