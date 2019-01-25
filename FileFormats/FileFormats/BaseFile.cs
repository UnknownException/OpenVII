using FileFormats.ArchiveFormats;

namespace FileFormats.FileFormats
{
    public abstract class BaseFile : IFile
    {
        public string Name { get; set; }
        public uint Offset { get; set; }
        public string DataName { get; set; }
        public long DataOffset { get; set; }
        public uint DataSize { get; set; }

        protected readonly IArchive _fileContainer = null;
        public BaseFile(IArchive fileContainer)
        {
            _fileContainer = fileContainer;
        }
    }
}
