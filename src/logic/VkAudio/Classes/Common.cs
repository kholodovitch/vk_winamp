namespace VkAudio.Classes
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public static class Common
    {
        private static NativeWindow _PlayerAccessorWindow;
        private static NativeWindow _PlayerWindow;

        public static IWebProxy GetRequestProxy()
        {
            Settings settings = Settings.Load();
            IWebProxy proxy = null;
            if (settings.UseProxy)
            {
                if (settings.UseIeProxy)
                {
                    return WebRequest.DefaultWebProxy;
                }
                proxy = new WebProxy(settings.ProxyAddress, settings.ProxyPort);
                if (!settings.ProxyNeedAuth)
                {
                    return proxy;
                }
                if (settings.ProxyUseWindowsAuth)
                {
                    (proxy as WebProxy).UseDefaultCredentials = true;
                    return proxy;
                }
                proxy.Credentials = new NetworkCredential(settings.ProxyLogin, settings.DecryptProxyPass());
            }
            return proxy;
        }

        [DllImport("user32.dll", SetLastError=true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public static string CachePath
        {
            get
            {
                return Path.Combine(WorkFilesDirectory, "cache.xml");
            }
        }

        public static NativeWindow PlayerAccessorWindow
        {
            get
            {
                if (_PlayerAccessorWindow == null)
                {
                    _PlayerAccessorWindow = new NativeWindow();
                    _PlayerAccessorWindow.AssignHandle(PlayerHandle);
                }
                return _PlayerAccessorWindow;
            }
        }

        public static IntPtr PlayerHandle
        {
            [CompilerGenerated]
            get
            {
                return <PlayerHandle>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PlayerHandle>k__BackingField = value;
            }
        }

        public static NativeWindow PlayerWindow
        {
            get
            {
                if (_PlayerWindow == null)
                {
                    _PlayerWindow = new NativeWindow();
                    _PlayerWindow.AssignHandle(PlayerWindowHandle);
                }
                return _PlayerWindow;
            }
        }

        public static IntPtr PlayerWindowHandle
        {
            [CompilerGenerated]
            get
            {
                return <PlayerWindowHandle>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <PlayerWindowHandle>k__BackingField = value;
            }
        }

        public static bool ProcessStreaming
        {
            [CompilerGenerated]
            get
            {
                return <ProcessStreaming>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ProcessStreaming>k__BackingField = value;
            }
        }

        public static string SongsCachePath
        {
            get
            {
                return Path.Combine(WorkFilesDirectory, "SongsCache/");
            }
        }

        public static bool UseUnicode
        {
            [CompilerGenerated]
            get
            {
                return <UseUnicode>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <UseUnicode>k__BackingField = value;
            }
        }

        public static string WorkFilesDirectory
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VkAudio", "Winamp");
            }
        }
    }
}

