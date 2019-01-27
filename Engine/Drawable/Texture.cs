using Shared;
using Shared.Engine.Drawable;
using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Veldrid.SPIRV;

namespace Engine.Drawable
{
    internal class Texture : ITexture
    {
        private readonly Veldrid.Texture _texture;
        private readonly DataBuffer _textureBuffer;

        public Texture(Veldrid.GraphicsDevice graphicsDevice, TextureFormat textureFormat, DataBuffer textureBuffer)
        {
            _texture = graphicsDevice.ResourceFactory.CreateTexture(Veldrid.TextureDescription.Texture2D(textureFormat.Width, textureFormat.Height, 1, 1,
                        Veldrid.PixelFormat.R8_G8_B8_A8_UNorm, Veldrid.TextureUsage.Sampled));

            _textureBuffer = textureBuffer;
            graphicsDevice.UpdateTexture(_texture, textureBuffer.GetIntPtr(), textureBuffer.GetSize(), 0, 0, 0, textureFormat.Width, textureFormat.Height, 1, 0, 0);
        }

        public void Draw(object objCommandList)
        {
            if (!objCommandList.GetType().IsSubclassOf(typeof(Veldrid.CommandList)))
            {
                throw new ArgumentException("Given argument is not of type CommandList");
            }

            var commandList = (Veldrid.CommandList)objCommandList;
        }
    }
}
