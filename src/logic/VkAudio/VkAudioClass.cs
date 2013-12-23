// Type: VkAudio.VkAudioClass
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using VkAudio.Classes;
using VkAudio.Forms;
using VkAudio.Properties;

namespace VkAudio
{
  [ClassInterface(ClassInterfaceType.AutoDual)]
  [Guid("594CC9F5-DA3C-4433-A8CD-20B70F19CCB7")]
  [ComVisible(true)]
  public class VkAudioClass
  {
    public static VkAudioClass Instance { get; private set; }

    public event VkAudioClass.PositionChangedDelegate PositionChanged;

    public event VkAudioClass.PositionRequestedDelegate PositionRequested;

    public event VkAudioClass.StateChangedDelegate StateChanged;

    public event VkAudioClass.LoadUrlDelegate LoadUrlInvoked;

    private void ShowExceptionDetails(Exception ex)
    {
      int num = (int) new BigMessageBoxForm("Ошибка", string.Format("Type:\r\n{0}\r\n\r\nMessage:\r\n{1}\r\n\r\nStackTrace:\r\n{2}", (object) ex.GetType(), (object) ex.Message, (object) ex.StackTrace)).ShowDialog((IWin32Window) Common.PlayerWindow);
    }

    private bool CheckSettings()
    {
      try
      {
        return Settings.Load() != null || new SettingsForm().ShowDialog() == DialogResult.OK;
      }
      catch (Exception ex)
      {
        this.ShowExceptionDetails(ex);
        return false;
      }
    }

    public void ShowSettings()
    {
      try
      {
        int num = (int) new SettingsForm().ShowDialog((IWin32Window) Common.PlayerWindow);
      }
      catch (Exception ex)
      {
        this.ShowExceptionDetails(ex);
      }
    }

    private void CacheData(List<VkSong> songs)
    {
      List<VkSong> list = Serialization.Deserialize<List<VkSong>>(Common.CachePath) ?? new List<VkSong>();
      list.RemoveAll((Predicate<VkSong>) (s => songs.Exists((Predicate<VkSong>) (ss => ss.ID == s.ID))));
      list.AddRange((IEnumerable<VkSong>) songs);
      Serialization.Serialize((object) list, Common.CachePath);
    }

    [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
    public string[] SearchSongs()
    {
      if (!this.CheckSettings())
        return new string[0];
      try
      {
        if (LoadingForm.Existed)
        {
          int num = (int) MessageBox.Show("Операция уже выполняется!");
          return new string[0];
        }
        else
        {
          SearchForm searchForm = new SearchForm();
          int num = (int) searchForm.ShowDialog((IWin32Window) Common.PlayerWindow);
          this.CacheData(searchForm.SelectedSongs);
          return this.GetArray(searchForm.SelectedSongs);
        }
      }
      catch (Exception ex)
      {
        this.ShowExceptionDetails(ex);
        return new string[0];
      }
    }

    public bool ReadAppendPrefix()
    {
      try
      {
        Settings settings = Settings.Load();
        if (settings == null)
          return true;
        else
          return settings.AddPrefix;
      }
      catch (Exception ex)
      {
        this.ShowExceptionDetails(ex);
        return true;
      }
    }

    private string[] GetArray(List<VkSong> songs)
    {
      string[] strArray = new string[songs.Count * 8];
      for (int index = 0; index < songs.Count; ++index)
      {
        strArray[index * 8] = songs[index].ID;
        strArray[index * 8 + 1] = songs[index].Artist;
        strArray[index * 8 + 2] = songs[index].Title;
        strArray[index * 8 + 3] = songs[index].Duration.ToString();
        strArray[index * 8 + 4] = songs[index].LyricsID.ToString();
        strArray[index * 8 + 5] = songs[index].OriginSearchUrl;
        strArray[index * 8 + 6] = songs[index].LocalFilePath;
        strArray[index * 8 + 7] = songs[index].URL;
      }
      return strArray;
    }

    private List<VkSong> GetSongs(string[] arr)
    {
      List<VkSong> list = new List<VkSong>();
      int index = 0;
      while (index < arr.Length)
      {
        list.Add(new VkSong()
        {
          ID = arr[index],
          Artist = arr[index + 1],
          Title = arr[index + 2],
          Duration = int.Parse(arr[index + 3]),
          LyricsID = string.IsNullOrEmpty(arr[index + 4]) ? 0 : int.Parse(arr[index + 4]),
          OriginSearchUrl = arr[index + 5],
          LocalFilePath = arr[index + 6],
          URL = arr[index + 7]
        });
        index += 8;
      }
      return list;
    }

    [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
    public string[] GetSongsCache()
    {
      try
      {
        return this.GetArray(Serialization.Deserialize<List<VkSong>>(Common.CachePath) ?? new List<VkSong>());
      }
      catch (Exception ex)
      {
        this.ShowExceptionDetails(ex);
        return new string[0];
      }
    }

    public void DownloadSongs(string[] data)
    {
      if (!this.CheckSettings())
        return;
      try
      {
        if (LoadingForm.Existed)
        {
          int num = (int) MessageBox.Show((IWin32Window) Common.PlayerWindow, "Операция уже выполняется!");
        }
        else
          VkSongListExtension.DownloadInterfaced(this.GetSongs(data), (IWin32Window) Common.PlayerWindow);
      }
      catch (Exception ex)
      {
        this.ShowExceptionDetails(ex);
      }
    }

    public void ShowLyrics(string Artist, string Title, int LyricsId, string OriginSearchUrl)
    {
      if (!this.CheckSettings())
        return;
      try
      {
        if (LoadingForm.Existed)
        {
          int num1 = (int) MessageBox.Show("Операция уже выполняется!");
        }
        else
        {
          int num2;
          VkTools.GetLyrics((Form) null, LyricsId, OriginSearchUrl, (Action<string>) (str => num2 = (int) new BigMessageBoxForm("Текст песни " + Artist + " - " + Title, str).ShowDialog()));
        }
      }
      catch (Exception ex)
      {
        this.ShowExceptionDetails(ex);
      }
    }

    [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
    public string[] CacheSongs(string[] data)
    {
      try
      {
        if (LoadingForm.Existed)
        {
          int num = (int) MessageBox.Show((IWin32Window) Common.PlayerWindow, "Операция уже выполняется!");
          return new string[0];
        }
        else
        {
          List<VkSong> songs = this.GetSongs(data);
          VkSongListExtension.Download(songs, (IWin32Window) Common.PlayerWindow, Common.SongsCachePath, true, (Action) null);
          this.CacheData(songs);
          return this.GetArray(Enumerable.ToList<VkSong>(Enumerable.Where<VkSong>((IEnumerable<VkSong>) songs, (Func<VkSong, bool>) (s => !string.IsNullOrEmpty(s.LocalFilePath)))));
        }
      }
      catch (Exception ex)
      {
        this.ShowExceptionDetails(ex);
        return new string[0];
      }
    }

    private string GetAssemblyResourceName(string assName)
    {
      if (assName.ToLower().Contains("taglib"))
        return "taglib_sharp";
      else
        return (string) null;
    }

    public void Init(IntPtr playerHandle, IntPtr playerWindowHandle, bool UseUnicode, bool ProcessStreaming)
    {
      VkAudioClass.Instance = this;
      Common.PlayerHandle = playerHandle;
      Common.PlayerWindowHandle = playerWindowHandle;
      Common.UseUnicode = UseUnicode;
      Common.ProcessStreaming = ProcessStreaming;
      AppDomain.CurrentDomain.AssemblyResolve += (ResolveEventHandler) ((s, e) =>
      {
        string local_0 = this.GetAssemblyResourceName(e.Name);
        if (local_0 == null)
          return (Assembly) null;
        else
          return Assembly.Load((byte[]) Resources.ResourceManager.GetObject(local_0));
      });
      if (!VisualStyleInformation.IsSupportedByOS || !VisualStyleInformation.IsEnabledByUser)
        return;
      Application.EnableVisualStyles();
    }

    public void InvokePositionChangedEvent(int Position)
    {
      if (this.PositionChanged == null)
        return;
      this.PositionChanged(Position);
    }

    public void InvokePositionRequested()
    {
      if (this.PositionRequested == null)
        return;
      this.PositionRequested();
    }

    public void InvokeStateChangedEvent(int State)
    {
      if (this.StateChanged == null)
        return;
      this.StateChanged(State);
    }

    public void InvokeLoadUrl(string url)
    {
      if (this.LoadUrlInvoked == null)
        return;
      this.LoadUrlInvoked(url);
    }

    public void CancelPreview()
    {
      if (SearchForm.CurrentForm == null)
        return;
      SearchForm.CurrentForm.StopPreviewPlayback(false);
    }

    public void PostionChangedCallback(int Position)
    {
      if (SearchForm.CurrentForm == null)
        return;
      SearchForm.CurrentForm.NotifyPositionChanged(Position);
    }

    public void StateChangedCallback(int State)
    {
      if (SearchForm.CurrentForm == null)
        return;
      SearchForm.CurrentForm.NotifyStateChanged(State);
    }

    public delegate void PositionChangedDelegate(int Position);

    public delegate void PositionRequestedDelegate();

    public delegate void StateChangedDelegate(int State);

    public delegate void LoadUrlDelegate([MarshalAs(UnmanagedType.BStr)] string url);
  }
}
