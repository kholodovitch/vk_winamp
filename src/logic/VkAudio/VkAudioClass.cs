namespace VkAudio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;
    using VkAudio.Classes;
    using VkAudio.Forms;
    using VkAudio.Properties;

    [ClassInterface(ClassInterfaceType.AutoDual), Guid("594CC9F5-DA3C-4433-A8CD-20B70F19CCB7"), ComVisible(true)]
    public class VkAudioClass
    {
        [CompilerGenerated]
        private static VkAudioClass <Instance>k__BackingField;

        public event LoadUrlDelegate LoadUrlInvoked;

        public event PositionChangedDelegate PositionChanged;

        public event PositionRequestedDelegate PositionRequested;

        public event StateChangedDelegate StateChanged;

        private void CacheData(List<VkSong> songs)
        {
            List<VkSong> list = Serialization.Deserialize<List<VkSong>>(Common.CachePath);
            if (list == null)
            {
                list = new List<VkSong>();
            }
            list.RemoveAll(delegate (VkSong s) {
                return songs.Exists(ss => ss.ID == s.ID);
            });
            list.AddRange(songs);
            Serialization.Serialize(list, Common.CachePath);
        }

        [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)]
        public string[] CacheSongs(string[] data)
        {
            try
            {
                if (LoadingForm.Existed)
                {
                    MessageBox.Show(Common.PlayerWindow, "Операция уже выполняется!");
                    return new string[0];
                }
                List<VkSong> songs = this.GetSongs(data);
                VkSongListExtension.Download(songs, Common.PlayerWindow, Common.SongsCachePath, true, null);
                this.CacheData(songs);
                if (CS$<>9__CachedAnonymousMethodDelegateb == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegateb = new Func<VkSong, bool>(null, (IntPtr) <CacheSongs>b__a);
                }
                return this.GetArray(Enumerable.ToList<VkSong>(Enumerable.Where<VkSong>(songs, CS$<>9__CachedAnonymousMethodDelegateb)));
            }
            catch (Exception exception)
            {
                this.ShowExceptionDetails(exception);
                return new string[0];
            }
        }

        public void CancelPreview()
        {
            if (SearchForm.CurrentForm != null)
            {
                SearchForm.CurrentForm.StopPreviewPlayback(false);
            }
        }

        private bool CheckSettings()
        {
            try
            {
                if ((Settings.Load() == null) && (new SettingsForm().ShowDialog() != DialogResult.OK))
                {
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                this.ShowExceptionDetails(exception);
                return false;
            }
        }

        public void DownloadSongs(string[] data)
        {
            if (this.CheckSettings())
            {
                try
                {
                    if (LoadingForm.Existed)
                    {
                        MessageBox.Show(Common.PlayerWindow, "Операция уже выполняется!");
                    }
                    else
                    {
                        VkSongListExtension.DownloadInterfaced(this.GetSongs(data), Common.PlayerWindow);
                    }
                }
                catch (Exception exception)
                {
                    this.ShowExceptionDetails(exception);
                }
            }
        }

        private string[] GetArray(List<VkSong> songs)
        {
            string[] strArray = new string[songs.Count * 8];
            for (int i = 0; i < songs.Count; i++)
            {
                strArray[i * 8] = songs[i].ID;
                strArray[(i * 8) + 1] = songs[i].Artist;
                strArray[(i * 8) + 2] = songs[i].Title;
                strArray[(i * 8) + 3] = songs[i].Duration.ToString();
                strArray[(i * 8) + 4] = songs[i].LyricsID.ToString();
                strArray[(i * 8) + 5] = songs[i].OriginSearchUrl;
                strArray[(i * 8) + 6] = songs[i].LocalFilePath;
                strArray[(i * 8) + 7] = songs[i].URL;
            }
            return strArray;
        }

        private string GetAssemblyResourceName(string assName)
        {
            if (assName.ToLower().Contains("taglib"))
            {
                return "taglib_sharp";
            }
            return null;
        }

        private List<VkSong> GetSongs(string[] arr)
        {
            List<VkSong> list = new List<VkSong>();
            for (int i = 0; i < arr.Length; i += 8)
            {
                VkSong item = new VkSong();
                item.ID = arr[i];
                item.Artist = arr[i + 1];
                item.Title = arr[i + 2];
                item.Duration = int.Parse(arr[i + 3]);
                item.LyricsID = string.IsNullOrEmpty(arr[i + 4]) ? 0 : int.Parse(arr[i + 4]);
                item.OriginSearchUrl = arr[i + 5];
                item.LocalFilePath = arr[i + 6];
                item.URL = arr[i + 7];
                list.Add(item);
            }
            return list;
        }

        [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)]
        public string[] GetSongsCache()
        {
            try
            {
                List<VkSong> songs = Serialization.Deserialize<List<VkSong>>(Common.CachePath);
                if (songs == null)
                {
                    songs = new List<VkSong>();
                }
                return this.GetArray(songs);
            }
            catch (Exception exception)
            {
                this.ShowExceptionDetails(exception);
                return new string[0];
            }
        }

        public void Init(IntPtr playerHandle, IntPtr playerWindowHandle, bool UseUnicode, bool ProcessStreaming)
        {
            Instance = this;
            Common.PlayerHandle = playerHandle;
            Common.PlayerWindowHandle = playerWindowHandle;
            Common.UseUnicode = UseUnicode;
            Common.ProcessStreaming = ProcessStreaming;
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object s, ResolveEventArgs e) {
                string name = this.GetAssemblyResourceName(e.Name);
                if (name == null)
                {
                    return null;
                }
                byte[] rawAssembly = (byte[]) Resources.ResourceManager.GetObject(name);
                return Assembly.Load(rawAssembly);
            };
            if (VisualStyleInformation.IsSupportedByOS && VisualStyleInformation.IsEnabledByUser)
            {
                Application.EnableVisualStyles();
            }
        }

        public void InvokeLoadUrl(string url)
        {
            if (this.LoadUrlInvoked != null)
            {
                this.LoadUrlInvoked(url);
            }
        }

        public void InvokePositionChangedEvent(int Position)
        {
            if (this.PositionChanged != null)
            {
                this.PositionChanged(Position);
            }
        }

        public void InvokePositionRequested()
        {
            if (this.PositionRequested != null)
            {
                this.PositionRequested();
            }
        }

        public void InvokeStateChangedEvent(int State)
        {
            if (this.StateChanged != null)
            {
                this.StateChanged(State);
            }
        }

        public void PostionChangedCallback(int Position)
        {
            if (SearchForm.CurrentForm != null)
            {
                SearchForm.CurrentForm.NotifyPositionChanged(Position);
            }
        }

        public bool ReadAppendPrefix()
        {
            try
            {
                Settings settings = Settings.Load();
                return ((settings == null) || settings.AddPrefix);
            }
            catch (Exception exception)
            {
                this.ShowExceptionDetails(exception);
                return true;
            }
        }

        [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType=VarEnum.VT_BSTR)]
        public string[] SearchSongs()
        {
            if (!this.CheckSettings())
            {
                return new string[0];
            }
            try
            {
                if (LoadingForm.Existed)
                {
                    MessageBox.Show("Операция уже выполняется!");
                    return new string[0];
                }
                SearchForm form = new SearchForm();
                form.ShowDialog(Common.PlayerWindow);
                this.CacheData(form.SelectedSongs);
                return this.GetArray(form.SelectedSongs);
            }
            catch (Exception exception)
            {
                this.ShowExceptionDetails(exception);
                return new string[0];
            }
        }

        private void ShowExceptionDetails(Exception ex)
        {
            string msg = string.Format("Type:\r\n{0}\r\n\r\nMessage:\r\n{1}\r\n\r\nStackTrace:\r\n{2}", ex.GetType(), ex.Message, ex.StackTrace);
            new BigMessageBoxForm("Ошибка", msg).ShowDialog(Common.PlayerWindow);
        }

        public void ShowLyrics(string Artist, string Title, int LyricsId, string OriginSearchUrl)
        {
            Action<string> successHandler = null;
            if (this.CheckSettings())
            {
                try
                {
                    if (LoadingForm.Existed)
                    {
                        MessageBox.Show("Операция уже выполняется!");
                    }
                    else
                    {
                        if (successHandler == null)
                        {
                            successHandler = delegate (string str) {
                                new BigMessageBoxForm("Текст песни " + Artist + " - " + Title, str).ShowDialog();
                            };
                        }
                        VkTools.GetLyrics(null, LyricsId, OriginSearchUrl, successHandler);
                    }
                }
                catch (Exception exception)
                {
                    this.ShowExceptionDetails(exception);
                }
            }
        }

        public void ShowSettings()
        {
            try
            {
                new SettingsForm().ShowDialog(Common.PlayerWindow);
            }
            catch (Exception exception)
            {
                this.ShowExceptionDetails(exception);
            }
        }

        public void StateChangedCallback(int State)
        {
            if (SearchForm.CurrentForm != null)
            {
                SearchForm.CurrentForm.NotifyStateChanged(State);
            }
        }

        public static VkAudioClass Instance
        {
            [CompilerGenerated]
            get
            {
                return <Instance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                <Instance>k__BackingField = value;
            }
        }

        public delegate void LoadUrlDelegate([MarshalAs(UnmanagedType.BStr)] string url);

        public delegate void PositionChangedDelegate(int Position);

        public delegate void PositionRequestedDelegate();

        public delegate void StateChangedDelegate(int State);
    }
}

