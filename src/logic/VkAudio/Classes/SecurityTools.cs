namespace VkAudio.Classes
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Management;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;

    public static class SecurityTools
    {
        private static byte[] LocalEncriptionKey;
        private static byte[] LocalHardwareKey;

        public static string DecryptString(byte[] bytes)
        {
            if ((bytes == null) || (bytes.Length == 0))
            {
                return "";
            }
            try
            {
                TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider {
                    IV = new byte[] { 0x38, 0x7d, 0x43, 11, 0, 12, 0x62, 0xc9 },
                    Key = GetEncryptionKey()
                };
                byte[] buffer = null;
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        stream2.Write(bytes, 0, bytes.Length);
                        stream2.FlushFinalBlock();
                        buffer = stream.ToArray();
                    }
                }
                return Encoding.UTF8.GetString(buffer);
            }
            catch
            {
                return "";
            }
        }

        public static byte[] EncryptString(string str)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider {
                IV = new byte[] { 0x38, 0x7d, 0x43, 11, 0, 12, 0x62, 0xc9 },
                Key = GetEncryptionKey()
            };
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] buffer2 = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    stream2.Write(bytes, 0, bytes.Length);
                    stream2.FlushFinalBlock();
                    buffer2 = stream.ToArray();
                }
            }
            return buffer2;
        }

        private static string GetBaseId()
        {
            return (GetHardwareId("Win32_BaseBoard", "Model") + GetHardwareId("Win32_BaseBoard", "Manufacturer") + GetHardwareId("Win32_BaseBoard", "Name") + GetHardwareId("Win32_BaseBoard", "SerialNumber"));
        }

        private static string GetBiosId()
        {
            return (GetHardwareId("Win32_BIOS", "Manufacturer") + GetHardwareId("Win32_BIOS", "SMBIOSBIOSVersion") + GetHardwareId("Win32_BIOS", "IdentificationCode") + GetHardwareId("Win32_BIOS", "SerialNumber") + GetHardwareId("Win32_BIOS", "ReleaseDate") + GetHardwareId("Win32_BIOS", "Version"));
        }

        public static byte[] GetBytes(string s)
        {
            s = s.Replace("-", "").Replace(" ", "");
            if ((s.Length % 2) != 0)
            {
                throw new ArgumentException("Строка неправильной длины!");
            }
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = byte.Parse(s[i].ToString() + s[i + 1].ToString(), NumberStyles.HexNumber);
            }
            return buffer;
        }

        public static byte[] GetEncryptionKey()
        {
            return (LocalEncriptionKey ?? (LocalEncriptionKey = GetEncryptionKey(GetHardwareKey())));
        }

        public static byte[] GetEncryptionKey(byte[] HardwareKey)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(0x4e3).GetBytes(Assembly.GetExecutingAssembly().GetName().Name));
            byte[] buffer2 = new byte[HardwareKey.Length];
            for (int i = 0; i < HardwareKey.Length; i++)
            {
                buffer2[i] = (byte) (HardwareKey[i] ^ buffer[i]);
            }
            return buffer2;
        }

        public static byte[] GetEncryptionKey(string HardwareKey)
        {
            return GetEncryptionKey(GetBytes(HardwareKey));
        }

        private static string GetHardwareId(string wmiClass, string wmiProperty)
        {
            string str = "";
            ManagementClass class2 = new ManagementClass(wmiClass);
            foreach (ManagementObject obj2 in class2.GetInstances())
            {
                if (str == "")
                {
                    try
                    {
                        return obj2[wmiProperty].ToString();
                    }
                    catch
                    {
                    }
                }
            }
            return str;
        }

        public static byte[] GetHardwareKey()
        {
            if (LocalHardwareKey == null)
            {
                string s = "\nBIOS >> " + GetBiosId() + "\nBASE >> " + GetBaseId();
                MD5 md = new MD5CryptoServiceProvider();
                byte[] bytes = Encoding.GetEncoding(0x4e3).GetBytes(s);
                return (LocalHardwareKey = md.ComputeHash(bytes));
            }
            return LocalHardwareKey;
        }
    }
}

