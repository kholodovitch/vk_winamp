using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using ApiCore;
using ApiCore.Audio;

namespace VkAudio.Classes
{
	public static class VkTools
	{
		private static HttpWebResponse thResp;

		static VkTools()
		{
		}

		public static void FreeResources()
		{
			if (VkTools.thResp == null)
				return;
			try
			{
				VkTools.thResp.Close();
			}
			catch
			{
			}
		}

		public static List<VkSong> GetSongsOffset(string query, string referer, int offset)
		{
			AudioFactory audioFactory = GetAudioFactory();
#if !DEBUG
#error Add logic
#endif
			return new List<VkSong>();
		}

		public static List<VkSong> GetGroupPersonSongsRest(string address)
		{
			AudioFactory audioFactory = GetAudioFactory();
#if !DEBUG
#error Add logic
#endif
			return new List<VkSong>();
		}

		public static List<VkSong> GetMySongsRest()
		{
			AudioFactory audioFactory = GetAudioFactory();
#if !DEBUG
#error Add logic
#endif
			return new List<VkSong>();
		}

		public static List<VkSong> GetGroupUserSongs(string address, out int totalCount)
		{
			AudioFactory audioFactory = GetAudioFactory();
#if !DEBUG
#error Add logic
#endif
			totalCount = 0;
			return new List<VkSong>();
		}

		public static List<VkSong> GetMySongs()
		{
			AudioFactory audioFactory = GetAudioFactory();
			List<VkSong> groupPersonSongsRest = audioFactory
				.Get(audioFactory.Manager.UserId, null, null)
				.Select(audioItem => new VkSong
					{
						Artist = audioItem.Artist,
						Duration = audioItem.Duration,
						ID = audioItem.Id.ToString(),
						Title = audioItem.Title,
						URL = audioItem.Url
					})
				.ToList();
			return groupPersonSongsRest;
		}

		public static List<VkSong> GetSongsFromUrl(string URL, out int totalCount)
		{
			AudioFactory audioFactory = GetAudioFactory();
#if !DEBUG
#error Add logic
#endif
			totalCount = 0;
			return new List<VkSong>();
		}

		public static List<VkSong> FindSongs(string SearchQuery, bool Performer, out int totalCount)
		{
			AudioFactory audioFactory = GetAudioFactory();
#if !DEBUG
#error Add logic
#endif
			totalCount = 0;
			return new List<VkSong>();
		}

		public static void GetLyrics(Form Owner, int LyricsId, string OriginUrl, Action<string> SuccessHandler)
		{
#if !DEBUG
#error Add logic
#endif
		}

		private static AudioFactory GetAudioFactory()
		{
			Settings settings = Settings.Load();
			SessionManager sm = new SessionManager(1928531, "audio");
			SessionInfo sessionInfo = sm.GetOAuthSession(settings.Email, settings.DecryptPass());
			var manager = new ApiManager(sessionInfo) { Timeout = 10000 };
			var audioFactory = new AudioFactory(manager);
			return audioFactory;
		}
	}
}