using System;
using System.Collections.Generic;
using System.Text;
using Shared;

namespace FileFormats.FileFormats
{
    public class UnknownFile : IFile
    {
        public string Name { get; set; }
        public uint Offset { get; set; }
        public string DataName { get; set; }
        public long DataOffset { get; set; }
        public uint DataSize { get; set; }
    }
}
