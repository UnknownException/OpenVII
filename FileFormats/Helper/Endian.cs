using System;
using System.Collections.Generic;
using System.Text;

namespace FileFormats.Helper
{
    public static class Endian
    {
        public static int Flip(int val)
        {
            return (int)((val & 0x000000FF) << 24 |
                (val & 0x0000FF00) << 8 |
                (val & 0xFF000000) >> 24 |
                (val & 0x00FF0000) >> 8);
        }
    }
}
