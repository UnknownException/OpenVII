using FileFormats.ArchiveFormats;
using FileFormats.Helper;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace FileFormats.FileFormats
{
    public class TexFile : BaseFile
    {
        // Based on Q-Gears TexFile specifications
        #region Q-Gears-TexFile-License
        /*
            -----------------------------------------------------------------------------
            The MIT License (MIT)
            Copyright (c) 2013-07-30 Tobias Peters <tobias.peters@kreativeffekt.at>
            Permission is hereby granted, free of charge, to any person obtaining a copy
            of this software and associated documentation files (the "Software"), to deal
            in the Software without restriction, including without limitation the rights
            to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
            copies of the Software, and to permit persons to whom the Software is
            furnished to do so, subject to the following conditions:
            The above copyright notice and this permission notice shall be included in
            all copies or substantial portions of the Software.
            THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
            IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
            FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
            AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
            LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
            OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
            THE SOFTWARE.
            -----------------------------------------------------------------------------
        */
        // Dec 11, 2014
        #endregion
        // https://github.com/q-gears/q-gears/blob/master/QGearsMain/src/data/QGearsTexFile.cpp
        // https://github.com/q-gears/q-gears/blob/master/QGearsMain/include/data/QGearsTexFile.h

        private Header _header;
        private List<RGBA> _imageData;

        public TexFile(IArchive fileContainer) : base(fileContainer)
        {
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BitData
        {
            public int ColorMin { get; set; }
            public int ColorMax { get; set; }
            public int AlphaMin { get; set; }
            public int AlphaMax { get; set; }
            public int PixelMin { get; set; }
            public int PixelMax { get; set; }
        }
        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ImageData
        {
            public int Bitdepth { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Pitch { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PaletteData
        {
            public int Flag { get; set; }
            public int IndexBits { get; set; }
            public int Index8Bit { get; set; }
            public int TotalColorCount { get; set; }
            public int ColorsPerPallete { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RGBAData
        {
            public int Red { get; set; }
            public int Green { get; set; }
            public int Blue { get; set; }
            public int Alpha { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PixelFormat
        {
            public int BitsPerPixel { get; set; }
            public int BytesPerPixel { get; set; }
            public RGBAData BitCount { get; set; }
            public RGBAData BitMask { get; set; }
            public RGBAData BitShift { get; set; }
            public RGBAData BitCountUnused { get; set; }
            public RGBAData Shades { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct Header
        {
            public int Version { get; set; }
            public int Unknown_0x04 { get; set; }
            public int ColorKeyFlag { get; set; }
            public int Unknown_0x0C { get; set; }
            public int Unknown_0x10 { get; set; }
            public BitData BitData { get; set; }
            public int PaletteType { get; set; }
            public int PaletteCount { get; set; }
            public int PaletteTotalColorCount { get; set; }
            public ImageData ImageData { get; set; }
            public int Unknown_0x48 { get; set; }
            public PaletteData PaletteData { get; set; }
            public int RuntimeDataPtrPaletteData { get; set; }
            public PixelFormat PixelFormat { get; set; }
            public int ColorKeyArrayFlag { get; set; }
            public int RuntimeDataPtrColorKeyArray { get; set; }
            public int ReferenceAlpha { get; set; }
            public int RuntimeData02 { get; set; }
            public int Unknown_0xCC { get; set; }
            public int RuntimeDataPaletteIndex { get; set; }
            public int RuntimeDataPtrImageData { get; set; }
            public int RuntimeData04 { get; set; }
            public int Unknown_06 { get; set; }
            public int Unknown_07 { get; set; }
            public int Unknown_08 { get; set; }
            public int Unknown_09 { get; set; }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RGBA
        {
            public byte Blue { get; set; }
            public byte Green { get; set; }
            public byte Red { get; set; }
            public byte Alpha { get; set; }
        }

        private void Read()
        {
            var headerContent = _fileContainer.Read(DataOffset, Marshal.SizeOf(_header));
            var fileOffset = DataOffset + Marshal.SizeOf(_header);

            _header = ByteToStruct.Convert<Header>(headerContent);

            if (_header.Version != 1)
            {
                throw new FileLoadException($"Unknown tex version: {_header.Version} in file {Name}");
            }

            if(_header.PaletteData.Flag == 1)
            {
                ReadPaletted(fileOffset);
            }
            else
            {
                throw new FileLoadException($"Can't handle tex files without palettedata. file {Name}");
            }
        }

        private void ReadPaletted(long fileOffset)
        {
            var _palette = new List<RGBA>();
            _imageData = new List<RGBA>();

            int totalColorCount = _header.PaletteData.TotalColorCount;
            if(totalColorCount == 0)
            {
                totalColorCount = _header.PaletteTotalColorCount;
            }

            // Read palettes
            int rgbaSize = Marshal.SizeOf(new RGBA());
            int paletteDataSize = totalColorCount * rgbaSize;
            var content = _fileContainer.Read(fileOffset, paletteDataSize);
            fileOffset += paletteDataSize;

            for(int i = 0; i < totalColorCount; ++i)
            {
                var palette = ByteToStruct.Convert<RGBA>(content, i * rgbaSize, rgbaSize);
                _palette.Add(palette);
            }

            // Read image indexes (references to palette)
            int imageSize = _header.ImageData.Width * _header.ImageData.Height;
            var indexData = _fileContainer.Read(fileOffset, imageSize);
            fileOffset += imageSize;

            if(fileOffset - DataOffset != DataSize)
            {
                throw new FileLoadException("Data read and data size do not match");
            }

            for (int i = 0; i < indexData.Length; ++i)
            {
                var index = indexData[i];
                if (index < _palette.Count)
                {
                    _imageData.Add(_palette[index]);
                }
                else
                {
                    throw new FileLoadException("Palette index out of bounds");
                }
            }
        }

        private WeakReference<DataBuffer> _imageCache;
        private bool _cached;
        public DataBuffer GetTextureBuffer()
        {
            if (_imageCache != null && _imageCache.TryGetTarget(out DataBuffer target))
            {
                return target;
            }

            if (_imageData.Count == 0)
            {
                Read();
            }

            int rgbaSize = Marshal.SizeOf(new RGBA());
            var buffer = new byte[_imageData.Count * rgbaSize];
            //TODO Optimize
            for(int i = 0; i < _imageData.Count; ++i)
            {  
                buffer[i * 4] = _imageData[i].Red;
                buffer[i*4+1] = _imageData[i].Green;
                buffer[i*4+2] = _imageData[i].Blue;
                buffer[i*4+3] = _imageData[i].Alpha;
            }

            _imageData.Clear();
            
            var dataBuffer = new DataBuffer(buffer);
            _imageCache = new WeakReference<DataBuffer>(dataBuffer);
            _cached = true;

            return dataBuffer;
        }

        public TextureFormat GetTextureFormat()
        {
            if(!_cached)
            {
                Read();
                _cached = true;
            }

            var textureFormat = new TextureFormat()
            {
                Height = (uint)_header.ImageData.Height,
                Width = (uint)_header.ImageData.Width                
            };

            return textureFormat;
        }
    }
}
