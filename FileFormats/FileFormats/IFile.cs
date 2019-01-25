using Shared;

namespace FileFormats.FileFormats
{
    public interface IFile
    {
        string Name { get; set; }
        uint Offset { get; set; }
        string DataName { get; set; }
        long DataOffset { get; set; }
        uint DataSize { get; set; }

//        DataBuffer GetDataBuffer();
    }
}
