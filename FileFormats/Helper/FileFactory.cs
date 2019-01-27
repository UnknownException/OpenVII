using FileFormats.ArchiveFormats;
using FileFormats.FileFormats;
using System;
using System.IO;

namespace FileFormats.Helper
{
    public static class FileFactory
    {
        public static IFile Create(string filename, IArchive archive)
        {
            if(!filename.Contains('.'))
            {
                return new UnknownFile();
            }

            string fileExtension = Path.GetExtension(filename.Trim('\0')).ToUpper().Substring(1);
            switch (fileExtension)
            {
                case "TEX":
                    {
                        return new TexFile(archive);
                    }
                case "MID":
                    {
                        return new MidiFile(archive);
                    }
                default:
                    {
                        return new UnknownFile();
                    }
            }
        }
    }
}
