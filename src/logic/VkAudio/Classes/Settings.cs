namespace VkAudio.Classes
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class Settings
    {
        public string DecryptPass()
        {
            return SecurityTools.DecryptString(this.EncryptedPass);
        }

        public string DecryptProxyPass()
        {
            return SecurityTools.DecryptString(this.EncryptedProxyPass);
        }

        private static string GetPath()
        {
            return Path.Combine(Common.WorkFilesDirectory, "settings.xml");
        }

        public static Settings Load()
        {
            return Serialization.Deserialize<Settings>(GetPath());
        }

        public void Save()
        {
            Serialization.Serialize(this, GetPath());
        }

        public bool AddPrefix { get; set; }

        public bool DownloadImmediate { get; set; }

        public string Email { get; set; }

        public byte[] EncryptedPass { get; set; }

        public byte[] EncryptedProxyPass { get; set; }

        public string ProxyAddress { get; set; }

        public string ProxyLogin { get; set; }

        public bool ProxyNeedAuth { get; set; }

        public int ProxyPort { get; set; }

        public bool ProxyUseWindowsAuth { get; set; }

        public bool UseIeProxy { get; set; }

        public bool UseProxy { get; set; }
    }
}

