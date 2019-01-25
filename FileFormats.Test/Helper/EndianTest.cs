using FileFormats.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileFormats.Test.Helper
{
    [TestClass]
    public class EndianTest
    {
        [TestMethod]
        public void Flip_ShouldReturnIntWithFlippedEndian()
        {
            // Arrange
            uint expected = 0xFF00FF00;
            uint input = 0x00FF00FF;

            // Act
            int result = Endian.Flip((int)input);

            // Assert
            Assert.AreEqual((int)expected, result);
        }
    }
}
