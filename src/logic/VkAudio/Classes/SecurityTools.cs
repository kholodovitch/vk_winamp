// Type: VkAudio.Classes.SecurityTools
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Globalization;
using System.IO;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace VkAudio.Classes
{
  public static class SecurityTools
  {
    private static byte[] LocalEncriptionKey;
    private static byte[] LocalHardwareKey;

    public static byte[] GetEncryptionKey()
    {
      return SecurityTools.LocalEncriptionKey ?? (SecurityTools.LocalEncriptionKey = SecurityTools.GetEncryptionKey(SecurityTools.GetHardwareKey()));
    }

    public static byte[] GetEncryptionKey(string HardwareKey)
    {
      return SecurityTools.GetEncryptionKey(SecurityTools.GetBytes(HardwareKey));
    }

    public static byte[] GetEncryptionKey(byte[] HardwareKey)
    {
      byte[] hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.GetEncoding(1251).GetBytes(Assembly.GetExecutingAssembly().GetName().Name));
      byte[] numArray = new byte[HardwareKey.Length];
      for (int index = 0; index < HardwareKey.Length; ++index)
        numArray[index] = (byte) ((uint) HardwareKey[index] ^ (uint) hash[index]);
      return numArray;
    }

    public static byte[] GetHardwareKey()
    {
      if (SecurityTools.LocalHardwareKey != null)
        return SecurityTools.LocalHardwareKey;
      string s = "\nBIOS >> " + SecurityTools.GetBiosId() + "\nBASE >> " + SecurityTools.GetBaseId();
      MD5 md5 = (MD5) new MD5CryptoServiceProvider();
      byte[] bytes = Encoding.GetEncoding(1251).GetBytes(s);
      return SecurityTools.LocalHardwareKey = md5.ComputeHash(bytes);
    }

    private static string GetHardwareId(string wmiClass, string wmiProperty)
    {
      string str = "";
      foreach (ManagementObject managementObject in new ManagementClass(wmiClass).GetInstances())
      {
        if (str == "")
        {
          try
          {
            str = managementObject[wmiProperty].ToString();
            break;
          }
          catch
          {
          }
        }
      }
      return str;
    }

    private static string GetBiosId()
    {
      return SecurityTools.GetHardwareId("Win32_BIOS", "Manufacturer") + SecurityTools.GetHardwareId("Win32_BIOS", "SMBIOSBIOSVersion") + SecurityTools.GetHardwareId("Win32_BIOS", "IdentificationCode") + SecurityTools.GetHardwareId("Win32_BIOS", "SerialNumber") + SecurityTools.GetHardwareId("Win32_BIOS", "ReleaseDate") + SecurityTools.GetHardwareId("Win32_BIOS", "Version");
    }

    private static string GetBaseId()
    {
      return SecurityTools.GetHardwareId("Win32_BaseBoard", "Model") + SecurityTools.GetHardwareId("Win32_BaseBoard", "Manufacturer") + SecurityTools.GetHardwareId("Win32_BaseBoard", "Name") + SecurityTools.GetHardwareId("Win32_BaseBoard", "SerialNumber");
    }

    public static byte[] GetBytes(string s)
    {
      s = s.Replace("-", "").Replace(" ", "");
      if (s.Length % 2 != 0)
        throw new ArgumentException("Строка неправильной длины!");
      byte[] numArray = new byte[s.Length / 2];
      int index = 0;
      while (index < s.Length)
      {
        numArray[index / 2] = byte.Parse(s[index].ToString() + s[index + 1].ToString(), NumberStyles.HexNumber);
        index += 2;
      }
      return numArray;
    }

    public static byte[] EncryptString(string str)
    {
      TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
      cryptoServiceProvider.IV = new byte[8]
      {
        (byte) 56,
        (byte) 125,
        (byte) 67,
        (byte) 11,
        (byte) 0,
        (byte) 12,
        (byte) 98,
        (byte) 201
      };
      cryptoServiceProvider.Key = SecurityTools.GetEncryptionKey();
      byte[] bytes = Encoding.UTF8.GetBytes(str);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write))
        {
          cryptoStream.Write(bytes, 0, bytes.Length);
          cryptoStream.FlushFinalBlock();
          return memoryStream.ToArray();
        }
      }
    }

    public static string DecryptString(byte[] bytes)
    {
      if (bytes != null)
      {
        if (bytes.Length != 0)
        {
          try
          {
            TripleDESCryptoServiceProvider cryptoServiceProvider = new TripleDESCryptoServiceProvider();
            cryptoServiceProvider.IV = new byte[8]
            {
              (byte) 56,
              (byte) 125,
              (byte) 67,
              (byte) 11,
              (byte) 0,
              (byte) 12,
              (byte) 98,
              (byte) 201
            };
            cryptoServiceProvider.Key = SecurityTools.GetEncryptionKey();
            byte[] bytes1 = (byte[]) null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
              using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, cryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write))
              {
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                bytes1 = memoryStream.ToArray();
              }
            }
            return Encoding.UTF8.GetString(bytes1);
          }
          catch
          {
            return "";
          }
        }
      }
      return "";
    }
  }
}
