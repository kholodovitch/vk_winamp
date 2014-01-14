using System;
using System.Linq;
using ApiCore;
using ApiCore.Audio;
using VkAudio;

namespace test_app
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			/*
			SessionManager sm = new SessionManager(1928531, "audio");
			SessionInfo sessionInfo = sm.GetOAuthSession(args[0], args[1]);
			var manager = new ApiManager(sessionInfo);
			manager.OnLog += manager_Log;
			manager.DebugMode = true;
			manager.Timeout = 10000;
			var audioFactory = new AudioFactory(manager);
			audioFactory
				.Get(sessionInfo.UserId, null, null)
				.OrderBy(x => x.Artist)
				.ToList()
				.ForEach(Console.WriteLine);
			*/

			var vkAudioClass = new VkAudioClass();
			vkAudioClass.Init(IntPtr.Zero, IntPtr.Zero, true, true);
			vkAudioClass.SearchSongs();
			Console.ReadLine();
		}

		private static void manager_Log(object sender, string msg)
		{
			Console.WriteLine(msg);
		}
	}
}
