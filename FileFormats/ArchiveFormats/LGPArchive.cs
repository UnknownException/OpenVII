using FileFormats.FileFormats;
using FileFormats.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileFormats.ArchiveFormats
{
    public class LGPArchive : IArchive
    {
        private BinaryReader _binaryReader { get; set; } = null;
        public List<IFile> FileList { get; set; } = null;

        public LGPArchive()
        {
            FileList = new List<IFile>();
        }

        public void Dispose()
        {
            _binaryReader?.Dispose();
        }

        public bool Open(string archivename)
        {
            if(archivename == null)
            {
                throw new ArgumentNullException("Archivename must be set");
            }

            if(!File.Exists(archivename))
            {
                return false;
            }

            try
            {
                _binaryReader = new BinaryReader(File.Open(archivename, FileMode.Open));
                ReadHeader();
            }
            catch (Exception e)
            {
                throw new FileLoadException(e.Message);
            }

            return true;
        }

        public IFile Find(string filename)
        {
            return FileList.FirstOrDefault(x => x.Name == filename);
        }

        private void ReadHeader()
        {
//            short unknown = _binaryReader.ReadInt16();
            string magicString = Encoding.UTF8.GetString(_binaryReader.ReadBytes(12));  // 0x00 0x00 STRING
            uint fileCount = _binaryReader.ReadUInt32();
        
            for(int i = 0; i < fileCount; ++i)
            {
                string filename = Encoding.UTF8.GetString(_binaryReader.ReadBytes(20)).Split('\0')[0]; // Some files contain error characters after the first \0
                uint fileOffset = _binaryReader.ReadUInt32();
                byte unknown1 = _binaryReader.ReadByte(); // 14
                byte unknown2 = _binaryReader.ReadByte(); // 0
                byte unknown3 = _binaryReader.ReadByte(); // 0
#if DEBUG
                Console.WriteLine($"-Name: {filename}");
#endif
                var currentPos = _binaryReader.BaseStream.Position;
                _binaryReader.BaseStream.Seek(fileOffset, SeekOrigin.Begin);
                string dataFile = Encoding.UTF8.GetString(_binaryReader.ReadBytes(20)).Split('\0')[0];
                uint dataSize = _binaryReader.ReadUInt32();

                IFile file = FileFactory.Create(filename, this);

#if DEBUG
                if (file.GetType() == typeof(UnknownFile))
                {
         //           Console.WriteLine("Failed to get format");
                }
#endif

                file.Name = filename.TrimEnd('\0');
                file.Offset = fileOffset;
                file.DataName = dataFile.TrimEnd('\0');
                file.DataSize = dataSize;
                file.DataOffset = _binaryReader.BaseStream.Position;

                FileList.Add(file);

                _binaryReader.BaseStream.Seek(currentPos, SeekOrigin.Begin);
            }
        }

        public byte[] Read(long offset, int size)
        {
            _binaryReader.BaseStream.Seek(offset, SeekOrigin.Begin);
            return _binaryReader.ReadBytes(size);
        }
    }
}
