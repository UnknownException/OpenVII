using System;
using System.Runtime.InteropServices;

namespace Shared
{
    public class DataBuffer
    {
        private readonly IntPtr _handle;
        private readonly uint _length;

        public DataBuffer(byte[] bytes)
        {
            _handle = Marshal.AllocHGlobal(bytes.Length);
            _length = (uint)bytes.Length;

            Marshal.Copy(bytes, 0, _handle, bytes.Length);
        }

        ~DataBuffer()
        {
            Marshal.FreeHGlobal(_handle);
        }

        public IntPtr GetIntPtr()
        {
            return _handle;
        }

        public uint GetSize()
        {
            return _length;
        }
    }
}