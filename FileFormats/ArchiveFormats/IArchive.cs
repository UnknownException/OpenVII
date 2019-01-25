using FileFormats.FileFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileFormats.ArchiveFormats
{
    public interface IArchive : IDisposable
    {
        bool Open(string archivename);

        IFile Find(string filename);

        byte[] Read(long offset, int size);
    }
}
