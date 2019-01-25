using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FileFormats.Helper
{
    public static class ByteToStruct
    {
        public static T Convert<T>(byte[] bytes) where T : struct
        {
            var handle = Marshal.AllocHGlobal(Marshal.SizeOf(new T()));
            Marshal.Copy(bytes, 0, handle, bytes.Length);
            var copy = Marshal.PtrToStructure<T>(handle);
            Marshal.FreeHGlobal(handle);   

            return copy;
        }

        public static T Convert<T>(byte[] bytes, int position, int length) where T : struct
        {
            var handle = Marshal.AllocHGlobal(Marshal.SizeOf(new T()));
            Marshal.Copy(bytes, position, handle, length);
            var copy = Marshal.PtrToStructure<T>(handle);
            Marshal.FreeHGlobal(handle);

            return copy;
        }
    }
}
