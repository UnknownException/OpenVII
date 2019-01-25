using System;
using FileFormats.ArchiveFormats;
using Shared;

namespace FileFormats.FileFormats
{
    public class MidiFile : BaseFile
    {
        private WeakReference<byte[]> _cache;
        public MidiFile(IArchive fileContainer) : base(fileContainer)
        {
            _cache = new WeakReference<byte[]>(null);
        }

        public byte[] GetBuffer()
        {
            if (_cache.TryGetTarget(out byte[] target) && target != null)
            {
                return target;
            }

            var buffer = _fileContainer.Read(DataOffset, (int)DataSize);
            _cache.SetTarget(buffer);

            return buffer;
        }
    }
}
