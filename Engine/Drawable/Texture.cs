using Shared;
using Shared.Engine.Drawable;
using System;

namespace EngineModule.Drawable
{
    internal class Texture : ITexture
    {
        private readonly Veldrid.GraphicsDevice _graphicsDevice;
        private readonly Veldrid.TextureDescription _textureDescription;
        private readonly Veldrid.Texture _texture;
        private readonly DataBuffer _textureBuffer;

        public Texture(Veldrid.GraphicsDevice graphicsDevice, TextureFormat textureFormat, DataBuffer textureBuffer)
        {
            _textureDescription = Veldrid.TextureDescription.Texture2D(textureFormat.Width, textureFormat.Height, 1, 1,
                                    Veldrid.PixelFormat.R8_G8_B8_A8_UInt, Veldrid.TextureUsage.Sampled | Veldrid.TextureUsage.Storage);

            _texture = graphicsDevice.ResourceFactory.CreateTexture(_textureDescription);

            _textureBuffer = textureBuffer;

            _graphicsDevice = graphicsDevice;
            _graphicsDevice.UpdateTexture(_texture, textureBuffer.GetIntPtr(), textureBuffer.GetSize(), 0, 0, 0, textureFormat.Width, textureFormat.Height, 0, 0, 0);
        }

        public void Draw(object objCommandList)
        {
            if (!objCommandList.GetType().IsSubclassOf(typeof(Veldrid.CommandList)))
            {
                throw new ArgumentException("Given argument is not of type CommandList");
            }

            var commandList = (Veldrid.CommandList)objCommandList;
            // Find out how to render with veldrid :)
        }
    }
}
