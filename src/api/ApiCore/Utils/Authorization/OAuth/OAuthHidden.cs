using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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

			HttpResult content = GetResponse(startUrl);
			string authorizeHtml = Encoding.UTF8.GetString(content.Array);

			// remove script element for prevent redirecting to remote page
			string authorizeHtmlLocal = authorizeHtml.Substring(0, authorizeHtml.IndexOf("<script")) + authorizeHtml.Substring(authorizeHtml.IndexOf("</script>") + "</script>".Length);
			authorizeHtmlLocal = authorizeHtmlLocal.Replace("href=\"/", "href=\"http://oauth.vk.com/");
			File.WriteAllText("index.html", authorizeHtmlLocal);

			string formHtml = GetFormHtml(authorizeHtml);
			string formAction = GetFormAction(formHtml);
			Dictionary<string, string> formParams = GetFormParams(formHtml);
			formParams["email"] = _email;
			formParams["pass"] = _pass;

			HttpResult postContent = GetResponse(formAction, x => WritePostData(x, formParams));
			if (postContent.ResponseUri.AbsolutePath != "/blank.html")
			{
				string postHtml = Encoding.UTF8.GetString(postContent.Array);
				File.WriteAllText("post.html", postHtml);

				formHtml = GetFormHtml(postHtml);
				formAction = GetFormAction(formHtml);
				formParams = GetFormParams(formHtml);

				postContent = GetResponse(formAction, x => WritePostData(x, formParams));
			}

			if (postContent.ResponseUri.AbsolutePath != @"/blank.html")
				throw new NotImplementedException();

			GetSessionData(postContent);

			return SessionData;
		}

		private void GetSessionData(HttpResult accessContent)
		{
			SessionData = new OAuthSessionInfo();

			string[] fragments = accessContent.ResponseUri.Fragment.Substring(1).Split('&');
			foreach (string fragment in fragments)
			{
				string[] fragmentNameValue = fragment.Split('=');
				switch (fragmentNameValue[0])
				{
					case "access_token":
						SessionData.Token = fragmentNameValue[1];
						break;
					case "expires_in":
						SessionData.Expire = int.Parse(fragmentNameValue[1]);
						break;
					case "user_id":
						SessionData.UserId = int.Parse(fragmentNameValue[1]);
						break;
				}
			}
		}

		private static string GetFormHtml(string html)
		{
			const string formStart = "<form";
			const string formEnd = "</form>";
			int startFormTag = html.IndexOf(formStart);
			int endFormTag = html.IndexOf(formEnd);
			return html.Substring(startFormTag, endFormTag + formEnd.Length - startFormTag);
		}

		private static string GetFormAction(string formHtml)
		{
			return Regex.Match(formHtml, @"action\=\""(.*)\""").Groups[1].Value;
		}

		private static Dictionary<string, string> GetFormParams(string formHtml)
		{
			const string formParamsPattern = @"\<input .+ name=\""(.*)\"" value=\""(.*)\"".*>";
			MatchCollection foundFormParams = Regex.Matches(formHtml, formParamsPattern);
			var formParams = new Dictionary<string, string>();
			foreach (Match match in foundFormParams)
				formParams[match.Groups[1].Value] = match.Groups[2].Value;
			return formParams;
		}

		private static void WritePostData(HttpWebRequest request, Dictionary<string, string> formParams)
		{
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";

			byte[] byteArray = Encoding.UTF8.GetBytes(DictToString(formParams));
			request.ContentLength = byteArray.Length;
			var datastream = request.GetRequestStream();
			datastream.Write(byteArray, 0, byteArray.Length);
			datastream.Close();
		}

		private static HttpResult GetResponse(string startUrl, Action<HttpWebRequest> requestAction = null)
		{
			var request = (HttpWebRequest) WebRequest.Create(startUrl);

			request.UserAgent = @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
			request.CookieContainer = Container;

			if (requestAction != null)
				requestAction(request);

			return GetRetArray(request);
		}

		private static HttpResult GetRetArray(HttpWebRequest request)
		{
			const int maxPacketByteSize = 64*1024;
			var ret = new HttpResult();

			using (var response = (HttpWebResponse) request.GetResponse())
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
							ret.Array = fileStream.ToArray();
						}
					}
				}
				ret.Encoding = response.ContentEncoding;
				ret.ResponseUri = response.ResponseUri;
			}
			return ret;
		}

		private static string DictToString(Dictionary<string, string> dict)
		{
			var builder = new StringBuilder();

			foreach (KeyValuePair<string, string> kvp in dict)
				builder.Append(kvp.Key + "=" + kvp.Value + "&");

			return builder.ToString();
		}

		private class HttpResult
		{
			public byte[] Array;
			public string Encoding;
			public Uri ResponseUri;
		}
	}
}