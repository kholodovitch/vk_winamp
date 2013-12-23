namespace VkAudio.Classes
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class Settings
    {
        [CompilerGenerated]
        private bool <AddPrefix>k__BackingField;
        [CompilerGenerated]
        private bool <DownloadImmediate>k__BackingField;
        [CompilerGenerated]
        private string <Email>k__BackingField;
        [CompilerGenerated]
        private byte[] <EncryptedPass>k__BackingField;
        [CompilerGenerated]
        private byte[] <EncryptedProxyPass>k__BackingField;
        [CompilerGenerated]
        private string <ProxyAddress>k__BackingField;
        [CompilerGenerated]
        private string <ProxyLogin>k__BackingField;
        [CompilerGenerated]
        private bool <ProxyNeedAuth>k__BackingField;
        [CompilerGenerated]
        private int <ProxyPort>k__BackingField;
        [CompilerGenerated]
        private bool <ProxyUseWindowsAuth>k__BackingField;
        [CompilerGenerated]
        private bool <UseIeProxy>k__BackingField;
        [CompilerGenerated]
        private bool <UseProxy>k__BackingField;

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

        public bool AddPrefix
        {
            [CompilerGenerated]
            get
            {
                return this.<AddPrefix>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<AddPrefix>k__BackingField = value;
            }
        }

        public bool DownloadImmediate
        {
            [CompilerGenerated]
            get
            {
                return this.<DownloadImmediate>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<DownloadImmediate>k__BackingField = value;
            }
        }

        public string Email
        {
            [CompilerGenerated]
            get
            {
                return this.<Email>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Email>k__BackingField = value;
            }
        }

        public byte[] EncryptedPass
        {
            [CompilerGenerated]
            get
            {
                return this.<EncryptedPass>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<EncryptedPass>k__BackingField = value;
            }
        }

        public byte[] EncryptedProxyPass
        {
            [CompilerGenerated]
            get
            {
                return this.<EncryptedProxyPass>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<EncryptedProxyPass>k__BackingField = value;
            }
        }

        public string ProxyAddress
        {
            [CompilerGenerated]
            get
            {
                return this.<ProxyAddress>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ProxyAddress>k__BackingField = value;
            }
        }

        public string ProxyLogin
        {
            [CompilerGenerated]
            get
            {
                return this.<ProxyLogin>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ProxyLogin>k__BackingField = value;
            }
        }

        public bool ProxyNeedAuth
        {
            [CompilerGenerated]
            get
            {
                return this.<ProxyNeedAuth>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ProxyNeedAuth>k__BackingField = value;
            }
        }

        public int ProxyPort
        {
            [CompilerGenerated]
            get
            {
                return this.<ProxyPort>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ProxyPort>k__BackingField = value;
            }
        }

        public bool ProxyUseWindowsAuth
        {
            [CompilerGenerated]
            get
            {
                return this.<ProxyUseWindowsAuth>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ProxyUseWindowsAuth>k__BackingField = value;
            }
        }

        public bool UseIeProxy
        {
            [CompilerGenerated]
            get
            {
                return this.<UseIeProxy>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<UseIeProxy>k__BackingField = value;
            }
        }

        public bool UseProxy
        {
            [CompilerGenerated]
            get
            {
                return this.<UseProxy>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<UseProxy>k__BackingField = value;
            }
        }
    }
}

