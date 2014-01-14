using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using HtmlAgilityPack;

namespace ApiCore.Utils.Authorization
{
	internal class OAuthHidden : IOAuthProvider
	{
		private static readonly CookieContainer Container = new CookieContainer();

		private readonly string _email;
		private readonly string _pass;

		public OAuthHidden(string email, string pass)
		{
			_email = email;
			_pass = pass;
		}

		public OAuthSessionInfo SessionData { get; private set; }

		public SessionInfo Authorize(int appId, string scope, string display)
		{
			const string blankHtml = "http://oauth.vk.com/blank.html";
			string startUrl = string.Format("http://oauth.vk.com/oauth/authorize?redirect_uri={0}&response_type=token&client_id={1}&scope={2}&display=wap&no_session=1", blankHtml, appId, scope);

			byte[] content = GetResponse(startUrl);
			var t = Encoding.UTF8.GetString(content);
			string formStart = "<form";
			int startFormTag = t.IndexOf(formStart) - formStart.Length;


			var htmlDocument = new HtmlDocument();
			using (var stream = new MemoryStream(content))
				htmlDocument.Load(stream);
			var t = htmlDocument.DocumentNode.SelectNodes("//form");
			t.ToString();


			return SessionData;
		}

		private static byte[] GetResponse(string startUrl)
		{
			var request = (HttpWebRequest) WebRequest.Create(startUrl);

			request.UserAgent = @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
			request.CookieContainer = Container;

			return GetRetArray(request);
		}

		private static byte[] GetRetArray(WebRequest request)
		{
			const int maxPacketByteSize = 64 * 1024;
			byte[] retArray = null;

			using (WebResponse response = request.GetResponse())
			{
				using (Stream respStream = response.GetResponseStream())
				{
					int read;
					var buffer = new byte[maxPacketByteSize];
					if ((read = respStream.Read(buffer, 0, buffer.Length)) > 0)
					{
						using (var fileStream = new MemoryStream())
						{
							fileStream.Write(buffer, 0, read);
							while ((read = respStream.Read(buffer, 0, buffer.Length)) > 0)
							{
								fileStream.Write(buffer, 0, read);
							}
							retArray = fileStream.ToArray();
						}
					}
				}
			}
			return retArray;
		}
	}
}