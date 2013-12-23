namespace VkAudio.Classes
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class VkSong
    {
        [CompilerGenerated]
        private string <Artist>k__BackingField;
        [CompilerGenerated]
        private int <Duration>k__BackingField;
        [CompilerGenerated]
        private string <ID>k__BackingField;
        [CompilerGenerated]
        private string <LocalFilePath>k__BackingField;
        [CompilerGenerated]
        private int <LyricsID>k__BackingField;
        [CompilerGenerated]
        private string <OriginSearchUrl>k__BackingField;
        [CompilerGenerated]
        private string <Title>k__BackingField;
        [CompilerGenerated]
        private string <URL>k__BackingField;

        public void GetLyrics(Form Owner, Action<string> SuccessHandler)
        {
            if (this.LyricsID > 0)
            {
                VkTools.GetLyrics(Owner, this.LyricsID, this.OriginSearchUrl, SuccessHandler);
            }
        }

        public override string ToString()
        {
            return (this.Artist + " - " + this.Title);
        }

        public string Artist
        {
            [CompilerGenerated]
            get
            {
                return this.<Artist>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Artist>k__BackingField = value;
            }
        }

        public int Duration
        {
            [CompilerGenerated]
            get
            {
                return this.<Duration>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Duration>k__BackingField = value;
            }
        }

        public string ID
        {
            [CompilerGenerated]
            get
            {
                return this.<ID>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ID>k__BackingField = value;
            }
        }

        public string LocalFilePath
        {
            [CompilerGenerated]
            get
            {
                return this.<LocalFilePath>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LocalFilePath>k__BackingField = value;
            }
        }

        public int LyricsID
        {
            [CompilerGenerated]
            get
            {
                return this.<LyricsID>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LyricsID>k__BackingField = value;
            }
        }

        public string OriginSearchUrl
        {
            [CompilerGenerated]
            get
            {
                return this.<OriginSearchUrl>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<OriginSearchUrl>k__BackingField = value;
            }
        }

        public string Title
        {
            [CompilerGenerated]
            get
            {
                return this.<Title>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Title>k__BackingField = value;
            }
        }

        public string URL
        {
            [CompilerGenerated]
            get
            {
                return this.<URL>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<URL>k__BackingField = value;
            }
        }
    }
}

