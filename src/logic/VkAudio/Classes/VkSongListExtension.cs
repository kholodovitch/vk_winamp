// Type: VkAudio.Classes.VkSongListExtension
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using TagLib;

namespace VkAudio.Classes
{
  public static class VkSongListExtension
  {
    public static void DownloadInterfaced(this List<VkSong> songs, IWin32Window Owner)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return;
      string selectedPath = folderBrowserDialog.SelectedPath;
      int num;
      VkSongListExtension.Download(songs, Owner, selectedPath, false, (Action) (() => num = (int) MessageBox.Show(songs.Count > 1 ? "Файлы сохранены!" : "Файл сохранён!")));
    }

    public static void Download(this List<VkSong> songs, IWin32Window Owner, string Path, bool IdsAsNames, Action SuccessHandler)
    {
      VkSong currentSong = (VkSong) null;
      WebClient client = (WebClient) null;
      Stream streamRemote = (Stream) null;
      Stream streamLocal = (Stream) null;
      TagLib.File file = (TagLib.File) null;
      object locker = new object();
      ThreadWorker.PerformOperation(Owner, (ThreadOperation) (() =>
      {
        if (!Directory.Exists(Path))
          Directory.CreateDirectory(Path);
        foreach (VkSong item_0 in songs)
        {
          string local_1 = "";
          lock (locker)
          {
            currentSong = item_0;
            ThreadWorker.UpdateStatus("Скачивание " + item_0.ToString());
            local_1 = Path.Combine(Path, IdsAsNames ? VkSongListExtension.ProcessPath(item_0.ID + ".mp3") : VkSongListExtension.ProcessPath(item_0.Artist + " - " + item_0.Title + ".mp3")).Replace("/", "\\");
            item_0.LocalFilePath = local_1;
          }
          string local_3 = Path.Combine(Common.SongsCachePath, VkSongListExtension.ProcessPath(item_0.ID + ".mp3")).Replace("/", "\\");
          if (System.IO.File.Exists(local_3))
          {
            if (local_1 != local_3)
              System.IO.File.WriteAllBytes(local_1, System.IO.File.ReadAllBytes(local_3));
            ThreadWorker.IncrementProgress(100);
          }
          else
          {
            HttpWebRequest local_5 = (HttpWebRequest) WebRequest.Create(new Uri(item_0.URL));
            local_5.Proxy = Common.GetRequestProxy();
            HttpWebResponse local_6 = (HttpWebResponse) local_5.GetResponse();
            local_6.Close();
            long local_7 = local_6.ContentLength;
            long local_8 = 0L;
            using (client = new WebClient())
            {
              client.Proxy = Common.GetRequestProxy();
              using (streamRemote = client.OpenRead(new Uri(item_0.URL)))
              {
                using (streamLocal = (Stream) new FileStream(local_1, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                  byte[] local_10 = new byte[102400];
                  int local_11 = 0;
                  int local_9_1;
                  while ((local_9_1 = streamRemote.Read(local_10, 0, local_10.Length)) > 0)
                  {
                    streamLocal.Write(local_10, 0, local_9_1);
                    local_8 += (long) local_9_1;
                    int local_12 = (int) Math.Round((double) local_8 / (double) local_7 * 100.0);
                    int local_13 = local_12 - local_11;
                    if (local_13 > 0)
                    {
                      ThreadWorker.IncrementProgress(local_13);
                      local_11 = local_12;
                    }
                  }
                  streamLocal.Close();
                }
                streamRemote.Close();
              }
            }
            if (!Common.UseUnicode)
            {
              TagLib.Id3v2.Tag.ForceDefaultEncoding = true;
              TagLib.Id3v2.Tag.DefaultEncoding = StringType.Latin1;
              ByteVector.UseBrokenLatin1Behavior = true;
            }
            file = TagLib.File.Create(local_1);
            file.Tag.Title = item_0.Title;
            file.Tag.Comment = "Saved from vk, say thanks to wdk";
            file.Tag.Performers = new string[1]
            {
              item_0.Artist
            };
            file.Save();
            file.Dispose();
          }
        }
      }), false, "Скачивание...", new int?(100 * songs.Count), (EventHandler) ((ls, le) =>
      {
        lock (locker)
        {
          if (client != null)
          {
            try
            {
              client.Dispose();
            }
            catch
            {
            }
          }
          if (streamRemote != null)
          {
            try
            {
              streamRemote.Close();
              streamRemote.Dispose();
            }
            catch
            {
            }
          }
          if (streamLocal != null)
          {
            try
            {
              streamLocal.Flush();
              streamLocal.Close();
              streamLocal.Dispose();
            }
            catch
            {
            }
          }
          if (file != null)
          {
            try
            {
              file.Dispose();
            }
            catch
            {
            }
          }
          if (!System.IO.File.Exists(currentSong.LocalFilePath))
            return;
          try
          {
            System.IO.File.Delete(currentSong.LocalFilePath);
            currentSong.LocalFilePath = "";
          }
          catch
          {
          }
        }
      }), SuccessHandler);
    }

    private static string ProcessPath(string path)
    {
      string str = path;
      foreach (char oldChar in Path.GetInvalidFileNameChars())
        str = str.Replace(oldChar, '_');
      foreach (char oldChar in Path.GetInvalidPathChars())
        str = str.Replace(oldChar, '_');
      return str;
    }
  }
}
