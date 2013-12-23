namespace VkAudio
{
    using System;
    using System.Runtime.InteropServices;

    [ClassInterface(ClassInterfaceType.AutoDual), Guid("2F59684E-15FD-48F7-A31B-99F545b85F7F"), ComVisible(true)]
    public class MarshalEx
    {
        public Delegate GetDelegateForFunctionPointer(IntPtr pUnmanagedFunction, Type type)
        {
            return Marshal.GetDelegateForFunctionPointer(pUnmanagedFunction, type);
        }
    }
}

