using FileFormats.FileFormats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FileFormats.Test.FileFormats
{
    [TestClass]
    public class TextFileTest
    {
        [TestMethod]
        public void Structs_ShouldBeOfExactSize()
        {
            // Arrange

            // Act
            int bitDataSize = Marshal.SizeOf(new TexFile.BitData());
            int imageDataSize = Marshal.SizeOf(new TexFile.ImageData());
            int paletteDataSize = Marshal.SizeOf(new TexFile.PaletteData());
            int rgbaDataSize = Marshal.SizeOf(new TexFile.RGBAData());
            int pixelFormatSize = Marshal.SizeOf(new TexFile.PixelFormat());
            int headerSize = Marshal.SizeOf(new TexFile.Header());
//            int textureFormatSize = Marshal.SizeOf(new TexFile.TextureFormat());

            // Assert
            Assert.AreEqual(bitDataSize, 24);
            Assert.AreEqual(imageDataSize, 16);
            Assert.AreEqual(paletteDataSize, 20);
            Assert.AreEqual(rgbaDataSize, 16);
            Assert.AreEqual(pixelFormatSize, 88);
            Assert.AreEqual(headerSize, 236);
//            Assert.AreEqual(textureFormatSize, 128);
        }
    }
}
