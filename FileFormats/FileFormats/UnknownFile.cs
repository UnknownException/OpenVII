using FileFormats.ArchiveFormats;
using System;

namespace FileFormats.FileFormats
{
    public class UnknownFile : BaseFile
    {
        public UnknownFile(IArchive fileContainer) : base(fileContainer)
        {
        }

        public byte[] GetBuffer()
        {
            return _fileContainer.Read(DataOffset, (int)DataSize);
        }
    }
}
