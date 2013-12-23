namespace VkAudio.Classes
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    public static class Serialization
    {
        public static T Deserialize<T>(string Path)
        {
            T local = default(T);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (FileStream stream = new FileStream(Path, FileMode.Open))
                {
                    local = (T) serializer.Deserialize(stream);
                }
            }
            catch
            {
            }
            return local;
        }

        public static void Serialize(object obj, string Path)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            if (!Directory.Exists(Path.GetDirectoryName(Path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Path));
            }
            using (FileStream stream = new FileStream(Path, FileMode.Create))
            {
                serializer.Serialize((Stream) stream, obj);
            }
        }
    }
}

