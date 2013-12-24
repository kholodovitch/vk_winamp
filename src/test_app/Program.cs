using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			SessionManager sm = new SessionManager(1928531, "audio");
			SessionInfo sessionInfo = sm.GetOAuthSession(args[0], args[1]);
			var manager = new ApiManager(sessionInfo);
			manager.OnLog += new ApiManagerLogHandler(manager_Log);
			manager.DebugMode = true;
			manager.Timeout = 10000;
			var audioFactory = new AudioFactory(manager);


			var vkAudioClass = new VkAudioClass();
			vkAudioClass.Init(IntPtr.Zero, IntPtr.Zero, true, true);
			vkAudioClass.SearchSongs();
		}

		private static void manager_Log(object sender, string msg)
		{
			Console.WriteLine(msg);
		}
	}
}
