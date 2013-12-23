// Type: VkAudio.Classes.Serialization
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System.IO;
using System.Xml.Serialization;

namespace VkAudio.Classes
{
  public static class Serialization
  {
    public static void Serialize(object obj, string path)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
      if (!Directory.Exists(Path.GetDirectoryName(path)))
        Directory.CreateDirectory(Path.GetDirectoryName(path));
      using (FileStream fileStream = new FileStream(path, FileMode.Create))
        xmlSerializer.Serialize((Stream) fileStream, obj);
    }

    public static T Deserialize<T>(string Path)
    {
      T obj = default (T);
      try
      {
        using (FileStream fileStream = new FileStream(Path, FileMode.Open))
          obj = (T) new XmlSerializer(typeof (T)).Deserialize((Stream) fileStream);
      }
      catch
      {
      }
      return obj;
    }
  }
}
