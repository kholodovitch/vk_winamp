// Type: VkAudio.Properties.Resources
// Assembly: VkAudio, Version=1.0.0.0, Culture=neutral, PublicKeyToken=c658c4eebe768023
// MVID: 92E79938-A83A-4CC7-8B72-61426CF41836
// Assembly location: D:\Projects\null\vk_winamp\build\VkAudio.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VkAudio.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  [DebuggerNonUserCode]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) Resources.resourceMan, (object) null))
          Resources.resourceMan = new ResourceManager("VkAudio.Properties.Resources", typeof (Resources).Assembly);
        return Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Resources.resourceCulture;
      }
      set
      {
        Resources.resourceCulture = value;
      }
    }

    internal static Bitmap ajax_loader
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("ajax_loader", Resources.resourceCulture);
      }
    }

    internal static Bitmap arrow_right
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("arrow_right", Resources.resourceCulture);
      }
    }

    internal static Bitmap pause
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("pause", Resources.resourceCulture);
      }
    }

    internal static Bitmap play
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("play", Resources.resourceCulture);
      }
    }

    internal static Bitmap stop
    {
      get
      {
        return (Bitmap) Resources.ResourceManager.GetObject("stop", Resources.resourceCulture);
      }
    }

    internal static byte[] taglib_sharp
    {
      get
      {
        return (byte[]) Resources.ResourceManager.GetObject("taglib_sharp", Resources.resourceCulture);
      }
    }

    internal Resources()
    {
    }
  }
}
