// Type: VkAudio.MarshalEx
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System;
using System.Runtime.InteropServices;

namespace VkAudio
{
  [ClassInterface(ClassInterfaceType.AutoDual)]
  [Guid("2F59684E-15FD-48F7-A31B-99F545b85F7F")]
  [ComVisible(true)]
  public class MarshalEx
  {
    public Delegate GetDelegateForFunctionPointer(IntPtr pUnmanagedFunction, Type type)
    {
      return Marshal.GetDelegateForFunctionPointer(pUnmanagedFunction, type);
    }
  }
}
