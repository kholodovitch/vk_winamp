// Type: VkAudio.Classes.VkTools
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

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

    private static HttpWebRequest GenerateVkRequest(string method, string sid, string url, string referer)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      httpWebRequest.Timeout = 300000;
      httpWebRequest.ReadWriteTimeout = 300000;
      httpWebRequest.CookieContainer = new CookieContainer();
      httpWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.2; en-US) AppleWebKit/531.0 (KHTML, like Gecko) Chrome/3.0.189.0 Safari/531.0";
      httpWebRequest.Method = method;
      httpWebRequest.Referer = referer;
      httpWebRequest.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
      httpWebRequest.Headers.Add("Accept-Encoding:gzip,deflate");
      httpWebRequest.Headers.Add("Accept-Language:ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
      httpWebRequest.Headers.Add("Accept-Charset:windows-1251,utf-8;q=0.7,*;q=0.3");
      httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
      httpWebRequest.Proxy = Common.GetRequestProxy();
      if (httpWebRequest.Proxy != null)
        ServicePointManager.Expect100Continue = false;
      if (!string.IsNullOrEmpty(sid))
      {
        httpWebRequest.CookieContainer.Add(new Cookie("remixchk", "5", "/", ".vk.com"));
        httpWebRequest.CookieContainer.Add(new Cookie("remixsid", sid, "/", ".vk.com"));
      }
      return httpWebRequest;
    }

    private static string SendVkGetRequest(string sid, string url, string referer)
    {
      string str = "";
      try
      {
        VkTools.thResp = (HttpWebResponse) VkTools.GenerateVkRequest("GET", sid, url, referer).GetResponse();
        using (StreamReader streamReader = new StreamReader(VkTools.thResp.GetResponseStream(), Encoding.GetEncoding(VkTools.thResp.CharacterSet)))
          str = streamReader.ReadToEnd();
      }
      catch (WebException ex)
      {
        if (ex.Message.Contains("404"))
          str = "";
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (Exception ex)
      {
        throw new Exception("Не удалось отправить запрос на " + url + "!", ex);
      }
      finally
      {
        VkTools.FreeResources();
      }
      return str;
    }

    private static string SendVkPostRequest(string sid, string url, string referer, string postData)
    {
      string str = "";
      try
      {
        HttpWebRequest httpWebRequest = VkTools.GenerateVkRequest("POST", sid, url, referer);
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) memoryStream))
          {
            streamWriter.Write(postData);
            ((TextWriter) streamWriter).Flush();
            httpWebRequest.ContentLength = (long) postData.Length;
            using (Stream requestStream = ((WebRequest) httpWebRequest).GetRequestStream())
              memoryStream.WriteTo(requestStream);
          }
        }
        VkTools.thResp = (HttpWebResponse) httpWebRequest.GetResponse();
        using (StreamReader streamReader = new StreamReader(VkTools.thResp.GetResponseStream(), Encoding.GetEncoding(VkTools.thResp.CharacterSet)))
          str = streamReader.ReadToEnd();
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (Exception ex)
      {
        throw new Exception("Не удалось отправить запрос на " + url + "!", ex);
      }
      finally
      {
        VkTools.FreeResources();
      }
      return str;
    }

    public static string GetRemixSid(string email, string pass)
    {
      string str1 = "";
      try
      {
        HttpWebRequest httpWebRequest = VkTools.GenerateVkRequest("POST", (string) null, "http://login.vk.com/", "http://vk.com");
        httpWebRequest.Headers.Add("Cache-Control:max-age=0");
        httpWebRequest.Headers.Add("Origin:http://vk.com");
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        string str2 = "email=" + email + "&pass=" + pass + "&expire=&act=login&q=1&al_frame=1&s=1";
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) memoryStream))
          {
            streamWriter.Write(str2);
            ((TextWriter) streamWriter).Flush();
            httpWebRequest.ContentLength = (long) str2.Length;
            using (Stream requestStream = ((WebRequest) httpWebRequest).GetRequestStream())
              memoryStream.WriteTo(requestStream);
          }
        }
        VkTools.thResp = (HttpWebResponse) httpWebRequest.GetResponse();
        str1 = VkTools.thResp.Cookies["remixsid"].Value;
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (Exception ex)
      {
        throw new Exception("Не удалось получить SID!", ex);
      }
      finally
      {
        VkTools.FreeResources();
      }
      if (str1.Length < 10)
        throw new UserException("Ошибка авторизации, проверьте правильность логина и пароля в настройках.");
      else
        return str1;
    }

    public static List<VkSong> GetSongsOffset(string query, string referer, int offset)
    {
      Settings settings = Settings.Load();
      return VkTools.ExtractSongs(VkTools.SendVkPostRequest(VkTools.GetRemixSid(settings.Email, settings.DecryptPass()), "http://vk.com/al_search.php", referer, string.Format("al=1&c%5Bq%5D={0}&c%5Bsection%5D=audio&offset={1}", (object) Uri.EscapeUriString(query), (object) offset)), referer);
    }

    private static List<VkSong> ExtractSongs(string html, string originalUrl)
    {
      List<VkSong> list = new List<VkSong>();
      Regex regex = new Regex("<div class=\"audio\\s*(no_actions)?\\s*(fl_l)?\" id=\"audio([^\"]+)\"");
      if (regex.Matches(html).Count == 0)
        return list;
      foreach (Match match1 in regex.Matches(html))
      {
        string str1 = match1.Groups[3].Value;
        int index = match1.Index;
        int startIndex1 = html.IndexOf("<a href=\"/search", index);
        int length1 = html.IndexOf("</a", startIndex1) - startIndex1;
        string str2 = VkTools.StripHtml(html.Substring(startIndex1, length1)).Trim();
        int startIndex2 = html.IndexOf("<span class=\"title", index);
        int length2 = html.IndexOf("<span class=\"user", startIndex2) - startIndex2;
        string str3 = VkTools.StripHtml(html.Substring(startIndex2, length2)).Trim();
        string str4 = new Regex("<input type=\"hidden\" id=\"audio_info" + str1 + "\" value=\"([^,]+),").Match(html).Groups[1].Value;
        int num1 = int.Parse(new Regex("<input type=\"hidden\" id=\"audio_info" + str1 + "\" value=\"[^,]+,([^\"]+)\"").Match(html).Groups[1].Value);
        Match match2 = new Regex("<span id=\"title" + str1 + "\">([^<>]*)</span>").Match(html);
        if (match2.Success)
        {
          string str5 = match2.Groups[1].Value;
        }
        else
        {
          string str6 = new Regex("<a href='javascript: showLyrics\\(" + str1 + ",\\d+\\);'>([^<>]*)</a>").Match(html).Groups[1].Value;
        }
        int num2 = 0;
        Match match3 = new Regex("showLyrics\\('" + str1 + "',(\\d+)").Match(html);
        if (match3.Success)
          num2 = int.Parse(match3.Groups[1].Value);
        string str7 = WebUtility.HtmlDecode(str3).Trim();
        string str8 = WebUtility.HtmlDecode(str2).Trim();
        while (str7.Contains("  "))
          str7 = str7.Replace("  ", " ");
        while (str8.Contains("  "))
          str8 = str8.Replace("  ", " ");
        list.Add(new VkSong()
        {
          ID = str1,
          Title = str7,
          Artist = str8,
          URL = str4,
          Duration = num1,
          LyricsID = num2,
          OriginSearchUrl = originalUrl
        });
      }
      return list;
    }

    public static List<VkSong> GetGroupPersonSongsRest(string address)
    {
      Settings settings = Settings.Load();
      string remixSid = VkTools.GetRemixSid(settings.Email, settings.DecryptPass());
      address = VkTools.NormalizePersonGroupAddress(address);
      string request = VkTools.SendVkGetRequest(remixSid, address, "http://vk.com");
      string link = (string) null;
      int? totalCount = new int?();
      VkTools.ExtractAudioPageData(request, out link, out totalCount);
      string str1 = "";
      foreach (char c in link)
      {
        if (char.IsDigit(c) || (int) c == 45)
          str1 = str1 + (object) c;
      }
      bool flag = str1.StartsWith("-");
      string str2 = str1.Replace("-", "");
      return VkTools.ExtractSongsRest(VkTools.SendVkPostRequest(remixSid, "http://vk.com/audio", "http://vk.com/audio", string.Format("act=load_audios_silent&al=1&gid={0}&id={1}", flag ? (object) str2 : (object) "0", flag ? (object) "0" : (object) str2)));
    }

    private static string NormalizePersonGroupAddress(string address)
    {
      address = address.ToLower();
      if (address.StartsWith("vk.com"))
        address = "http://" + address;
      else if (!address.StartsWith("http://"))
        address = "http://vk.com/" + address;
      return address;
    }

    public static List<VkSong> GetMySongsRest()
    {
      Settings settings = Settings.Load();
      string remixSid = VkTools.GetRemixSid(settings.Email, settings.DecryptPass());
      string str = new Regex("id:\\s*(\\d+),").Match(VkTools.SendVkGetRequest(remixSid, "http://vk.com", "")).Groups[1].Value;
      return VkTools.ExtractSongsRest(VkTools.SendVkPostRequest(remixSid, "http://vk.com/audio", "http://vk.com/audio", "act=load_audios_silent&al=1&gid=0&id=" + str));
    }

    private static List<VkSong> ExtractSongsRest(string json)
    {
      List<VkSong> list = new List<VkSong>();
      Regex regex = new Regex("\\['([^,]+)','([^,]+)','([^,]+)','([^,]+)','([^,]+)','([^,]+)','([^,]+)','([^,]*)'[^\\]]+\\]");
      if (regex.Matches(json).Count == 0)
        return list;
      foreach (Match match in regex.Matches(json))
        list.Add(new VkSong()
        {
          ID = match.Groups[1].Value + "_" + match.Groups[2].Value,
          Title = WebUtility.HtmlDecode(match.Groups[7].Value),
          Artist = WebUtility.HtmlDecode(match.Groups[6].Value),
          URL = match.Groups[3].Value,
          Duration = int.Parse(match.Groups[4].Value),
          LyricsID = string.IsNullOrEmpty(match.Groups[8].Value) ? 0 : int.Parse(match.Groups[8].Value),
          OriginSearchUrl = VkTools.GenerateUrl(WebUtility.HtmlDecode(match.Groups[6].Value + " - " + match.Groups[7].Value), false)
        });
      return list;
    }

    private static void ExtractAudioPageData(string html, out string link, out int? totalCount)
    {
      Regex regex = new Regex("<a href=\"(/audios[^\"]+)\"");
      if (!regex.Match(html).Success)
      {
        link = (string) null;
        totalCount = new int?(0);
      }
      else
      {
        link = "http://vk.com" + regex.Match(html).Groups[1].Value;
        int index = regex.Match(html).Index;
        int num1 = html.IndexOf("</span>", index);
        int startIndex = html.IndexOf("</span>", num1 + 1);
        int num2 = html.IndexOf("</div>", startIndex);
        string str = html.Substring(startIndex, num2 - startIndex);
        string s = "";
        foreach (char c in str)
        {
          if (char.IsDigit(c))
            s = s + (object) c;
        }
        if (!string.IsNullOrEmpty(s))
          totalCount = new int?(int.Parse(s));
        else
          totalCount = new int?();
      }
    }

    public static string GetGroupPersonAudioAddress(string address)
    {
      try
      {
        Settings settings = Settings.Load();
        string remixSid = VkTools.GetRemixSid(settings.Email, settings.DecryptPass());
        address = VkTools.NormalizePersonGroupAddress(address);
        string request = VkTools.SendVkGetRequest(remixSid, address, "http://vk.com");
        string link = (string) null;
        int? totalCount = new int?();
        VkTools.ExtractAudioPageData(request, out link, out totalCount);
        return link;
      }
      catch (ThreadAbortException ex)
      {
        return "";
      }
      catch (UserException ex)
      {
        throw ex;
      }
      catch
      {
        return "";
      }
    }

    public static List<VkSong> GetGroupUserSongs(string address, out int totalCount)
    {
      Settings settings = Settings.Load();
      string remixSid = VkTools.GetRemixSid(settings.Email, settings.DecryptPass());
      List<VkSong> list = (List<VkSong>) null;
      totalCount = 0;
      try
      {
        address = VkTools.NormalizePersonGroupAddress(address);
        string request = VkTools.SendVkGetRequest(remixSid, address, "http://vk.com");
        string link = (string) null;
        int? totalCount1 = new int?();
        VkTools.ExtractAudioPageData(request, out link, out totalCount1);
        if (string.IsNullOrEmpty(link))
          return (List<VkSong>) null;
        list = VkTools.ExtractSongs(VkTools.SendVkGetRequest(remixSid, link, address), link);
        totalCount = !totalCount1.HasValue ? Enumerable.Count<VkSong>((IEnumerable<VkSong>) list) : totalCount1.Value;
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (UserException ex)
      {
        throw ex;
      }
      catch
      {
      }
      return list;
    }

    public static List<VkSong> GetMySongs(out int totalCount)
    {
      Settings settings = Settings.Load();
      string remixSid = VkTools.GetRemixSid(settings.Email, settings.DecryptPass());
      List<VkSong> list = (List<VkSong>) null;
      totalCount = 0;
      string request1 = VkTools.SendVkGetRequest(remixSid, "http://vk.com/audio", "http://vk.com");
      try
      {
        list = VkTools.ExtractSongs(request1, "http://vk.com/audio");
        string request2 = VkTools.SendVkGetRequest(remixSid, "http://vk.com", "");
        int startat = request2.IndexOf("id=\"myprofile_table\"");
        string str1 = new Regex("<a href=\"([^\"]+)\"").Match(request2, startat).Groups[1].Value;
        string request3 = VkTools.SendVkGetRequest(remixSid, "http://vk.com" + str1, "");
        int startIndex1 = request3.IndexOf("profile_audios");
        int num1 = request3.IndexOf("</span>", startIndex1);
        int startIndex2 = request3.IndexOf("</span>", num1 + 1);
        int num2 = request3.IndexOf("</div>", startIndex2);
        string str2 = request3.Substring(startIndex2, num2 - startIndex2);
        string s = "";
        foreach (char c in str2)
        {
          if (char.IsDigit(c))
            s = s + (object) c;
        }
        totalCount = string.IsNullOrEmpty(s) ? Enumerable.Count<VkSong>((IEnumerable<VkSong>) list) : int.Parse(s);
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (UserException ex)
      {
        throw ex;
      }
      catch
      {
      }
      return list;
    }

    public static List<VkSong> GetSongsFromUrl(string URL, out int totalCount)
    {
      Settings settings = Settings.Load();
      string remixSid = VkTools.GetRemixSid(settings.Email, settings.DecryptPass());
      List<VkSong> list = (List<VkSong>) null;
      totalCount = 0;
      try
      {
        string request = VkTools.SendVkGetRequest(remixSid, URL, "http://vk.com/audio.php");
        list = VkTools.ExtractSongs(request, URL);
        if (list.Count == 0)
          totalCount = 0;
        int startIndex = request.IndexOf("<div id=\"summary\" class=\"summary\">");
        int num = request.IndexOf("<a id", startIndex);
        string str = VkTools.StripHtml(request.Substring(startIndex, num - startIndex)).Replace("\n", " ").Replace("\r", " ");
        while (str.Contains("  "))
          str = str.Replace("  ", " ");
        string input = str.Trim();
        Regex regex = new Regex("[^\\d] ([\\d ]+) [^\\d]");
        totalCount = int.Parse(regex.Match(input).Groups[1].Value.Replace(" ", "").Replace(",", "").Replace(".", ""));
      }
      catch (ThreadAbortException ex)
      {
      }
      catch (UserException ex)
      {
        throw ex;
      }
      catch
      {
      }
      return list;
    }

    private static string StripHtml(string html)
    {
      return Regex.Replace(html, "<(.|\\n)*?>", string.Empty);
    }

    public static string GenerateUrl(string SearchQuery, bool Performer)
    {
      return "http://vk.com/search?" + Uri.EscapeUriString("c[q]") + "=" + Uri.EscapeUriString(SearchQuery) + "&" + Uri.EscapeUriString("c[section]") + "=audio" + (Performer ? "&" + Uri.EscapeUriString("c[performer]") + "=1" : "");
    }

    public static List<VkSong> FindSongs(string SearchQuery, bool Performer, out int totalCount)
    {
      return VkTools.GetSongsFromUrl(VkTools.GenerateUrl(SearchQuery, Performer), out totalCount);
    }

    public static void GetLyrics(Form Owner, int LyricsId, string OriginUrl, Action<string> SuccessHandler)
    {
      string result = "";
      ThreadWorker.PerformOperation((IWin32Window) Owner, (ThreadOperation) (() =>
      {
        Settings local_0 = Settings.Load();
        result = WebUtility.HtmlDecode(VkTools.SendVkPostRequest(VkTools.GetRemixSid(local_0.Email, local_0.DecryptPass()), "http://vk.com/audio.php", OriginUrl, "act=getLyrics&lid=" + LyricsId.ToString())).Replace("<br>", "\r\n");
      }), false, "Получение текста песни", new int?(), (EventHandler) ((ls, le) => result = (string) null), (Action) (() => SuccessHandler(result)));
    }
  }
}
