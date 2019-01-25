using System;
using FileFormats.ArchiveFormats;
using Shared;

namespace FileFormats.FileFormats
{
    public class MidiFile : BaseFile
    {
        private WeakReference<DataBuffer> _cache;
        public MidiFile(IArchive fileContainer) : base(fileContainer)
        {
            _cache = new WeakReference<DataBuffer>(null);
        }

        public DataBuffer GetBuffer()
        {
            if (_cache.TryGetTarget(out DataBuffer target) && target != null)
            {
                return target;
            }

            var buffer = new DataBuffer(_fileContainer.Read(DataOffset, (int)DataSize));
            _cache.SetTarget(buffer);

            return buffer;
        }
    }
}
