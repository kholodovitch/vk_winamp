using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkAudio;

namespace test_app
{
	class Program
	{
		static void Main(string[] args)
		{
			var vkAudioClass = new VkAudioClass();
			vkAudioClass.Init(IntPtr.Zero, IntPtr.Zero, true, true);
			vkAudioClass.SearchSongs();
		}
	}
}
