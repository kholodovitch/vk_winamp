// Type: VkAudio.Classes.VkSong
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Windows.Forms;

namespace VkAudio.Classes
{
  public class VkSong
  {
    public string ID { get; set; }

    public string Artist { get; set; }

    public string Title { get; set; }

    public string URL { get; set; }

    public int Duration { get; set; }

    public int LyricsID { get; set; }

    public string OriginSearchUrl { get; set; }

    public string LocalFilePath { get; set; }

    public void GetLyrics(Form Owner, Action<string> SuccessHandler)
    {
      if (this.LyricsID <= 0)
        return;
      VkTools.GetLyrics(Owner, this.LyricsID, this.OriginSearchUrl, SuccessHandler);
    }

    public override string ToString()
    {
      return this.Artist + " - " + this.Title;
    }
  }
}
