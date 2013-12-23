namespace VkAudio.Classes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public static class VkTools
    {
        private static HttpWebResponse thResp;

        private static void ExtractAudioPageData(string html, out string link, out int? totalCount)
        {
            Regex regex = new Regex("<a href=\"(/audios[^\"]+)\"");
            if (!regex.Match(html).Success)
            {
                link = null;
                totalCount = 0;
            }
            else
            {
                link = "http://vk.com" + regex.Match(html).Groups[1].Value;
                int index = regex.Match(html).Index;
                index = html.IndexOf("</span>", index);
                index = html.IndexOf("</span>", (int) (index + 1));
                int num2 = html.IndexOf("</div>", index);
                string str = html.Substring(index, num2 - index);
                string str2 = "";
                foreach (char ch in str)
                {
                    if (char.IsDigit(ch))
                    {
                        str2 = str2 + ch;
                    }
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    totalCount = new int?(int.Parse(str2));
                }
                else
                {
                    totalCount = 0;
                }
            }
        }

        private static List<VkSong> ExtractSongs(string html, string originalUrl)
        {
            List<VkSong> list = new List<VkSong>();
            Regex regex = new Regex("<div class=\"audio\\s*(no_actions)?\\s*(fl_l)?\" id=\"audio([^\"]+)\"");
            if (regex.Matches(html).Count != 0)
            {
                foreach (Match match in regex.Matches(html))
                {
                    string str = match.Groups[3].Value;
                    int index = match.Index;
                    int startIndex = html.IndexOf("<a href=\"/search", index);
                    int length = html.IndexOf("</a", startIndex) - startIndex;
                    string str3 = StripHtml(html.Substring(startIndex, length)).Trim();
                    int num4 = html.IndexOf("<span class=\"title", index);
                    int num5 = html.IndexOf("<span class=\"user", num4) - num4;
                    string str5 = StripHtml(html.Substring(num4, num5)).Trim();
                    Regex regex2 = new Regex("<input type=\"hidden\" id=\"audio_info" + str + "\" value=\"([^,]+),");
                    string str6 = regex2.Match(html).Groups[1].Value;
                    Regex regex3 = new Regex("<input type=\"hidden\" id=\"audio_info" + str + "\" value=\"[^,]+,([^\"]+)\"");
                    int num6 = int.Parse(regex3.Match(html).Groups[1].Value);
                    Regex regex4 = new Regex("<span id=\"title" + str + "\">([^<>]*)</span>");
                    Match match2 = regex4.Match(html);
                    if (match2.Success)
                    {
                        string text1 = match2.Groups[1].Value;
                    }
                    else
                    {
                        regex4 = new Regex(@"<a href='javascript: showLyrics\(" + str + @",\d+\);'>([^<>]*)</a>");
                        string text2 = regex4.Match(html).Groups[1].Value;
                    }
                    int num7 = 0;
                    Match match3 = new Regex(@"showLyrics\('" + str + @"',(\d+)").Match(html);
                    if (match3.Success)
                    {
                        num7 = int.Parse(match3.Groups[1].Value);
                    }
                    str5 = WebUtility.HtmlDecode(str5).Trim();
                    str3 = WebUtility.HtmlDecode(str3).Trim();
                    while (str5.Contains("  "))
                    {
                        str5 = str5.Replace("  ", " ");
                    }
                    while (str3.Contains("  "))
                    {
                        str3 = str3.Replace("  ", " ");
                    }
                    VkSong item = new VkSong {
                        ID = str,
                        Title = str5,
                        Artist = str3,
                        URL = str6,
                        Duration = num6,
                        LyricsID = num7,
                        OriginSearchUrl = originalUrl
                    };
                    list.Add(item);
                }
            }
            return list;
        }

        private static List<VkSong> ExtractSongsRest(string json)
        {
            List<VkSong> list = new List<VkSong>();
            Regex regex = new Regex(@"\['([^,]+)','([^,]+)','([^,]+)','([^,]+)','([^,]+)','([^,]+)','([^,]+)','([^,]*)'[^\]]+\]");
            if (regex.Matches(json).Count != 0)
            {
                foreach (Match match in regex.Matches(json))
                {
                    VkSong item = new VkSong {
                        ID = match.Groups[1].Value + "_" + match.Groups[2].Value,
                        Title = WebUtility.HtmlDecode(match.Groups[7].Value),
                        Artist = WebUtility.HtmlDecode(match.Groups[6].Value),
                        URL = match.Groups[3].Value,
                        Duration = int.Parse(match.Groups[4].Value),
                        LyricsID = string.IsNullOrEmpty(match.Groups[8].Value) ? 0 : int.Parse(match.Groups[8].Value),
                        OriginSearchUrl = GenerateUrl(WebUtility.HtmlDecode(match.Groups[6].Value + " - " + match.Groups[7].Value), false)
                    };
                    list.Add(item);
                }
            }
            return list;
        }

        public static List<VkSong> FindSongs(string SearchQuery, bool Performer, out int totalCount)
        {
            return GetSongsFromUrl(GenerateUrl(SearchQuery, Performer), out totalCount);
        }

        public static void FreeResources()
        {
            if (thResp != null)
            {
                try
                {
                    thResp.Close();
                }
                catch
                {
                }
            }
        }

        public static string GenerateUrl(string SearchQuery, bool Performer)
        {
            return ("http://vk.com/search?" + Uri.EscapeUriString("c[q]") + "=" + Uri.EscapeUriString(SearchQuery) + "&" + Uri.EscapeUriString("c[section]") + "=audio" + (Performer ? ("&" + Uri.EscapeUriString("c[performer]") + "=1") : ""));
        }

        private static HttpWebRequest GenerateVkRequest(string method, string sid, string url, string referer)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.Timeout = 0x493e0;
            request.ReadWriteTimeout = 0x493e0;
            request.CookieContainer = new CookieContainer();
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/531.0 (KHTML, like Gecko) Chrome/3.0.189.0 Safari/531.0";
            request.Method = method;
            request.Referer = referer;
            request.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            request.Headers.Add("Accept-Encoding:gzip,deflate");
            request.Headers.Add("Accept-Language:ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            request.Headers.Add("Accept-Charset:windows-1251,utf-8;q=0.7,*;q=0.3");
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Proxy = Common.GetRequestProxy();
            if (request.Proxy != null)
            {
                ServicePointManager.Expect100Continue = false;
            }
            if (!string.IsNullOrEmpty(sid))
            {
                request.CookieContainer.Add(new Cookie("remixchk", "5", "/", ".vk.com"));
                request.CookieContainer.Add(new Cookie("remixsid", sid, "/", ".vk.com"));
            }
            return request;
        }

        public static string GetGroupPersonAudioAddress(string address)
        {
            string str4;
            try
            {
                Settings settings = Settings.Load();
                string remixSid = GetRemixSid(settings.Email, settings.DecryptPass());
                address = NormalizePersonGroupAddress(address);
                string html = SendVkGetRequest(remixSid, address, "http://vk.com");
                string link = null;
                int? totalCount = null;
                ExtractAudioPageData(html, out link, out totalCount);
                str4 = link;
            }
            catch (ThreadAbortException)
            {
                str4 = "";
            }
            catch (UserException exception)
            {
                throw exception;
            }
            catch
            {
                str4 = "";
            }
            return str4;
        }

        public static List<VkSong> GetGroupPersonSongsRest(string address)
        {
            Settings settings = Settings.Load();
            string remixSid = GetRemixSid(settings.Email, settings.DecryptPass());
            address = NormalizePersonGroupAddress(address);
            string html = SendVkGetRequest(remixSid, address, "http://vk.com");
            string link = null;
            int? totalCount = null;
            ExtractAudioPageData(html, out link, out totalCount);
            string str4 = "";
            foreach (char ch in link)
            {
                if (char.IsDigit(ch) || (ch == '-'))
                {
                    str4 = str4 + ch;
                }
            }
            bool flag = str4.StartsWith("-");
            str4 = str4.Replace("-", "");
            return ExtractSongsRest(SendVkPostRequest(remixSid, "http://vk.com/audio", "http://vk.com/audio", string.Format("act=load_audios_silent&al=1&gid={0}&id={1}", flag ? str4 : "0", flag ? "0" : str4)));
        }

        public static List<VkSong> GetGroupUserSongs(string address, out int totalCount)
        {
            Settings settings = Settings.Load();
            string remixSid = GetRemixSid(settings.Email, settings.DecryptPass());
            List<VkSong> source = null;
            totalCount = 0;
            try
            {
                address = NormalizePersonGroupAddress(address);
                string html = SendVkGetRequest(remixSid, address, "http://vk.com");
                string link = null;
                int? nullable = null;
                ExtractAudioPageData(html, out link, out nullable);
                if (string.IsNullOrEmpty(link))
                {
                    return null;
                }
                source = ExtractSongs(SendVkGetRequest(remixSid, link, address), link);
                if (nullable.HasValue)
                {
                    totalCount = nullable.Value;
                    return source;
                }
                totalCount = source.Count<VkSong>();
            }
            catch (ThreadAbortException)
            {
            }
            catch (UserException exception)
            {
                throw exception;
            }
            catch
            {
            }
            return source;
        }

        public static void GetLyrics(Form Owner, int LyricsId, string OriginUrl, Action<string> SuccessHandler)
        {
            <>c__DisplayClass5 class2;
            string result = "";
            ThreadWorker.PerformOperation(Owner, delegate {
                Settings settings = Settings.Load();
                string str2 = SendVkPostRequest(GetRemixSid(settings.Email, settings.DecryptPass()), "http://vk.com/audio.php", OriginUrl, "act=getLyrics&lid=" + LyricsId.ToString());
                result = WebUtility.HtmlDecode(str2).Replace("<br>", "\r\n");
            }, false, "Получение текста песни", null, (EventHandler) ((ls, le) => (result = null)), new Action(class2, (IntPtr) this.<GetLyrics>b__4));
        }

        public static List<VkSong> GetMySongs(out int totalCount)
        {
            Settings settings = Settings.Load();
            string remixSid = GetRemixSid(settings.Email, settings.DecryptPass());
            List<VkSong> source = null;
            totalCount = 0;
            string html = SendVkGetRequest(remixSid, "http://vk.com/audio", "http://vk.com");
            try
            {
                source = ExtractSongs(html, "http://vk.com/audio");
                html = SendVkGetRequest(remixSid, "http://vk.com", "");
                int index = html.IndexOf("id=\"myprofile_table\"");
                Regex regex = new Regex("<a href=\"([^\"]+)\"");
                html = SendVkGetRequest(remixSid, "http://vk.com" + regex.Match(html, index).Groups[1].Value, "");
                index = html.IndexOf("profile_audios");
                index = html.IndexOf("</span>", index);
                index = html.IndexOf("</span>", (int) (index + 1));
                int num2 = html.IndexOf("</div>", index);
                string str4 = html.Substring(index, num2 - index);
                string str5 = "";
                foreach (char ch in str4)
                {
                    if (char.IsDigit(ch))
                    {
                        str5 = str5 + ch;
                    }
                }
                if (!string.IsNullOrEmpty(str5))
                {
                    totalCount = int.Parse(str5);
                    return source;
                }
                totalCount = source.Count<VkSong>();
            }
            catch (ThreadAbortException)
            {
            }
            catch (UserException exception)
            {
                throw exception;
            }
            catch
            {
            }
            return source;
        }

        public static List<VkSong> GetMySongsRest()
        {
            Settings settings = Settings.Load();
            string remixSid = GetRemixSid(settings.Email, settings.DecryptPass());
            string input = SendVkGetRequest(remixSid, "http://vk.com", "");
            Regex regex = new Regex(@"id:\s*(\d+),");
            string str3 = regex.Match(input).Groups[1].Value;
            return ExtractSongsRest(SendVkPostRequest(remixSid, "http://vk.com/audio", "http://vk.com/audio", "act=load_audios_silent&al=1&gid=0&id=" + str3));
        }

        public static string GetRemixSid(string email, string pass)
        {
            string str = "";
            try
            {
                HttpWebRequest request = GenerateVkRequest("POST", null, "http://login.vk.com/", "http://vk.com");
                request.Headers.Add("Cache-Control:max-age=0");
                request.Headers.Add("Origin:http://vk.com");
                request.ContentType = "application/x-www-form-urlencoded";
                string str2 = "email=" + email + "&pass=" + pass + "&expire=&act=login&q=1&al_frame=1&s=1";
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(str2);
                        writer.Flush();
                        request.ContentLength = str2.Length;
                        using (Stream stream2 = request.GetRequestStream())
                        {
                            stream.WriteTo(stream2);
                        }
                    }
                }
                thResp = (HttpWebResponse) request.GetResponse();
                str = thResp.Cookies["remixsid"].Value;
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                throw new Exception("Не удалось получить SID!", exception);
            }
            finally
            {
                FreeResources();
            }
            if (str.Length < 10)
            {
                throw new UserException("Ошибка авторизации, проверьте правильность логина и пароля в настройках.");
            }
            return str;
        }

        public static List<VkSong> GetSongsFromUrl(string URL, out int totalCount)
        {
            Settings settings = Settings.Load();
            string remixSid = GetRemixSid(settings.Email, settings.DecryptPass());
            List<VkSong> list = null;
            totalCount = 0;
            try
            {
                string html = SendVkGetRequest(remixSid, URL, "http://vk.com/audio.php");
                list = ExtractSongs(html, URL);
                if (list.Count == 0)
                {
                    totalCount = 0;
                }
                int index = html.IndexOf("<div id=\"summary\" class=\"summary\">");
                int num2 = html.IndexOf("<a id", index);
                string input = StripHtml(html.Substring(index, num2 - index)).Replace("\n", " ").Replace("\r", " ");
                while (input.Contains("  "))
                {
                    input = input.Replace("  ", " ");
                }
                input = input.Trim();
                Regex regex = new Regex(@"[^\d] ([\d ]+) [^\d]");
                totalCount = int.Parse(regex.Match(input).Groups[1].Value.Replace(" ", "").Replace(",", "").Replace(".", ""));
            }
            catch (ThreadAbortException)
            {
            }
            catch (UserException exception)
            {
                throw exception;
            }
            catch
            {
            }
            return list;
        }

        public static List<VkSong> GetSongsOffset(string query, string referer, int offset)
        {
            Settings settings = Settings.Load();
            return ExtractSongs(SendVkPostRequest(GetRemixSid(settings.Email, settings.DecryptPass()), "http://vk.com/al_search.php", referer, string.Format("al=1&c%5Bq%5D={0}&c%5Bsection%5D=audio&offset={1}", Uri.EscapeUriString(query), offset)), referer);
        }

        private static string NormalizePersonGroupAddress(string address)
        {
            address = address.ToLower();
            if (address.StartsWith("vk.com"))
            {
                address = "http://" + address;
                return address;
            }
            if (!address.StartsWith("http://"))
            {
                address = "http://vk.com/" + address;
            }
            return address;
        }

        private static string SendVkGetRequest(string sid, string url, string referer)
        {
            string str = "";
            try
            {
                thResp = (HttpWebResponse) GenerateVkRequest("GET", sid, url, referer).GetResponse();
                using (StreamReader reader = new StreamReader(thResp.GetResponseStream(), Encoding.GetEncoding(thResp.CharacterSet)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException exception)
            {
                if (exception.Message.Contains("404"))
                {
                    str = "";
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception2)
            {
                throw new Exception("Не удалось отправить запрос на " + url + "!", exception2);
            }
            finally
            {
                FreeResources();
            }
            return str;
        }

        private static string SendVkPostRequest(string sid, string url, string referer, string postData)
        {
            try
            {
                HttpWebRequest request = GenerateVkRequest("POST", sid, url, referer);
                request.ContentType = "application/x-www-form-urlencoded";
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(postData);
                        writer.Flush();
                        request.ContentLength = postData.Length;
                        using (Stream stream2 = request.GetRequestStream())
                        {
                            stream.WriteTo(stream2);
                        }
                    }
                }
                thResp = (HttpWebResponse) request.GetResponse();
                using (StreamReader reader = new StreamReader(thResp.GetResponseStream(), Encoding.GetEncoding(thResp.CharacterSet)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                throw new Exception("Не удалось отправить запрос на " + url + "!", exception);
            }
            finally
            {
                FreeResources();
            }
            return "";
        }

        private static string StripHtml(string html)
        {
            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }
    }
}

