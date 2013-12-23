namespace VkAudio.Classes
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class VkSong
    {
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

        public string Artist { get; set; }

        public int Duration { get; set; }

        public string ID { get; set; }

        public string LocalFilePath { get; set; }

        public int LyricsID { get; set; }

        public string OriginSearchUrl { get; set; }

        public string Title { get; set; }

        public string URL { get; set; }
    }
}

