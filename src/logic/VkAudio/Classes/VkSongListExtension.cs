namespace VkAudio.Classes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using TagLib;
    using TagLib.Id3v2;

    public static class VkSongListExtension
    {
        public static void Download(this List<VkSong> songs, IWin32Window Owner, string Path, bool IdsAsNames, Action SuccessHandler)
        {
            VkSong currentSong = null;
            WebClient client = null;
            Stream streamRemote = null;
            Stream streamLocal = null;
            TagLib.File file = null;
            object locker = new object();
            ThreadWorker.PerformOperation(Owner, delegate {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                foreach (VkSong song in songs)
                {
                    string path = "";
                    lock (locker)
                    {
                        currentSong = song;
                        ThreadWorker.UpdateStatus("Скачивание " + song.ToString());
                        path = Path.Combine(Path, IdsAsNames ? ProcessPath(song.ID + ".mp3") : ProcessPath(song.Artist + " - " + song.Title + ".mp3")).Replace("/", @"\");
                        song.LocalFilePath = path;
                    }
                    string str2 = Path.Combine(Common.SongsCachePath, ProcessPath(song.ID + ".mp3")).Replace("/", @"\");
                    if (System.IO.File.Exists(str2))
                    {
                        if (path != str2)
                        {
                            System.IO.File.WriteAllBytes(path, System.IO.File.ReadAllBytes(str2));
                        }
                        ThreadWorker.IncrementProgress(100);
                    }
                    else
                    {
                        Uri requestUri = new Uri(song.URL);
                        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                        request.Proxy = Common.GetRequestProxy();
                        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                        response.Close();
                        long contentLength = response.ContentLength;
                        long num2 = 0L;
                        using (client = new WebClient())
                        {
                            client.Proxy = Common.GetRequestProxy();
                            using (streamRemote = client.OpenRead(new Uri(song.URL)))
                            {
                                using (streamLocal = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    int count = 0;
                                    byte[] buffer = new byte[0x19000];
                                    int num4 = 0;
                                    while ((count = streamRemote.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        streamLocal.Write(buffer, 0, count);
                                        num2 += count;
                                        int num5 = (int) Math.Round((double) ((((float) num2) / ((float) contentLength)) * 100f));
                                        int num6 = num5 - num4;
                                        if (num6 > 0)
                                        {
                                            ThreadWorker.IncrementProgress(num6);
                                            num4 = num5;
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
                        file = TagLib.File.Create(path);
                        file.Tag.Title = song.Title;
                        file.Tag.Comment = "Saved from vk, say thanks to wdk";
                        file.Tag.Performers = new string[] { song.Artist };
                        file.Save();
                        file.Dispose();
                    }
                }
            }, false, "Скачивание...", new int?(100 * songs.Count), delegate (object ls, EventArgs le) {
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
                    if (System.IO.File.Exists(currentSong.LocalFilePath))
                    {
                        try
                        {
                            System.IO.File.Delete(currentSong.LocalFilePath);
                            currentSong.LocalFilePath = "";
                        }
                        catch
                        {
                        }
                    }
                }
            }, SuccessHandler);
        }

        public static void DownloadInterfaced(this List<VkSong> songs, IWin32Window Owner)
        {
            Action successHandler = null;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = dialog.SelectedPath;
                if (successHandler == null)
                {
                    <>c__DisplayClass2 class2;
                    successHandler = new Action(class2, (IntPtr) this.<DownloadInterfaced>b__0);
                }
                songs.Download(Owner, selectedPath, false, successHandler);
            }
        }

        private static string ProcessPath(string path)
        {
            string str = path;
            foreach (char ch in Path.GetInvalidFileNameChars())
            {
                str = str.Replace(ch, '_');
            }
            foreach (char ch2 in Path.GetInvalidPathChars())
            {
                str = str.Replace(ch2, '_');
            }
            return str;
        }
    }
}

