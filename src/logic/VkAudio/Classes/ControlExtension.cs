namespace VkAudio.Classes
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [Extension]
    public static class ControlExtension
    {
        [Extension]
        public static void InvokeOperation(Control TargetControl, ThreadOperation o)
        {
            if (TargetControl.InvokeRequired)
            {
                TargetControl.Invoke(new InvokeOperationDelegate(ControlExtension.InvokeOperation), new object[] { TargetControl, o });
            }
            else
            {
                o();
            }
        }

        private delegate void InvokeOperationDelegate(Control TargetControl, ThreadOperation o);
    }
}

