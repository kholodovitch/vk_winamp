namespace VkAudio.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), CompilerGenerated, DebuggerNonUserCode]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        internal static Bitmap ajax_loader
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ajax_loader", resourceCulture);
            }
        }

        internal static Bitmap arrow_right
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("arrow_right", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static Bitmap pause
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("pause", resourceCulture);
            }
        }

        internal static Bitmap play
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("play", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("VkAudio.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap stop
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("stop", resourceCulture);
            }
        }

        internal static byte[] taglib_sharp
        {
            get
            {
                return (byte[]) ResourceManager.GetObject("taglib_sharp", resourceCulture);
            }
        }
    }
}

