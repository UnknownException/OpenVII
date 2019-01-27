using Shared;
using Shared.Engine.Drawable;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using Veldrid.SPIRV;

namespace Engine.Drawable
{
    internal class Image : IImage
    {
        private DataBuffer _textureBuffer;

        private Veldrid.Texture _texture;
        private Veldrid.DeviceBuffer _vertexBuffer;
        private Veldrid.DeviceBuffer _indexBuffer;
        private Veldrid.TextureView _textureView;
        private Veldrid.ResourceLayout _resourceLayout;
        private Veldrid.ResourceSet _resourceSet;
        private Veldrid.Shader[] _shaders;
        private Veldrid.Pipeline _pipeline;


        public Image(Veldrid.GraphicsDevice graphicsDevice, TextureFormat textureFormat, DataBuffer textureBuffer)
        {
            // Perhaps create this somewhere else...?
            // Texture begin
            _texture = graphicsDevice.ResourceFactory.CreateTexture(Veldrid.TextureDescription.Texture2D(textureFormat.Width, textureFormat.Height, 1, 1,
                        Veldrid.PixelFormat.R8_G8_B8_A8_UNorm, Veldrid.TextureUsage.Sampled));

            _textureBuffer = textureBuffer;
            graphicsDevice.UpdateTexture(_texture, textureBuffer.GetIntPtr(), textureBuffer.GetSize(), 0, 0, 0, textureFormat.Width, textureFormat.Height, 1, 0, 0);
            // Texture end

            CreateVertexBuffer(graphicsDevice);
            CreateIndexBuffer(graphicsDevice);
            CreateResourceSet(graphicsDevice);
            CreateShaders(graphicsDevice);
            CreatePipeline(graphicsDevice);
        }

        public void Dispose()
        {
            _pipeline?.Dispose();
            if(_shaders != null)
            {
                foreach(var shader in _shaders)
                {
                    shader?.Dispose();
                }
            }
            _resourceSet?.Dispose();
            _resourceLayout?.Dispose();
            _textureView?.Dispose();
            _textureView?.Dispose();
            _indexBuffer?.Dispose();
            _vertexBuffer?.Dispose();
        }

        private void CreateVertexBuffer(Veldrid.GraphicsDevice graphicsDevice)
        {
            ImageVertex[] quadVertices =
            {
                new ImageVertex(new Vector2(-1f, 1f), new Vector2(0,0)),
                new ImageVertex(new Vector2(1f, 1f), new Vector2(1,0)),
                new ImageVertex(new Vector2(-1f, -1f), new Vector2(0,1)),
                new ImageVertex(new Vector2(1f, -1f), new Vector2(1,1))
            };

            _vertexBuffer = graphicsDevice.ResourceFactory.CreateBuffer(new Veldrid.BufferDescription(
                (uint)(quadVertices.Length * Marshal.SizeOf(new ImageVertex())),
                Veldrid.BufferUsage.VertexBuffer
            ));
            graphicsDevice.UpdateBuffer(_vertexBuffer, 0, quadVertices);
        }

        private void CreateIndexBuffer(Veldrid.GraphicsDevice graphicsDevice)
        {
            ushort[] quadIndices = { 0, 1, 2, 3 };

            _indexBuffer = graphicsDevice.ResourceFactory.CreateBuffer(new Veldrid.BufferDescription(
                (uint)(quadIndices.Length * sizeof(ushort)),
                Veldrid.BufferUsage.IndexBuffer
            ));
            graphicsDevice.UpdateBuffer(_indexBuffer, 0, quadIndices);
        }

        private void CreateResourceSet(Veldrid.GraphicsDevice graphicsDevice)
        {
            _resourceLayout = graphicsDevice.ResourceFactory.CreateResourceLayout(new Veldrid.ResourceLayoutDescription(
                    new Veldrid.ResourceLayoutElementDescription("ImageTexture", Veldrid.ResourceKind.TextureReadOnly, Veldrid.ShaderStages.Fragment),
                    new Veldrid.ResourceLayoutElementDescription("ImageSampler", Veldrid.ResourceKind.Sampler, Veldrid.ShaderStages.Fragment)
            ));

            _textureView = graphicsDevice.ResourceFactory.CreateTextureView(_texture);
            _resourceSet = graphicsDevice.ResourceFactory.CreateResourceSet(new Veldrid.ResourceSetDescription(
                _resourceLayout,
                _textureView,
                graphicsDevice.PointSampler
            ));
        }

        private void CreateShaders(Veldrid.GraphicsDevice graphicsDevice)
        {
            var vertexShader = new Veldrid.ShaderDescription(
                Veldrid.ShaderStages.Vertex,
                Encoding.UTF8.GetBytes(VertexShader),
                "main"
            );

            var fragmentShader = new Veldrid.ShaderDescription(
                Veldrid.ShaderStages.Fragment,
                Encoding.UTF8.GetBytes(FragmentShader),
                "main"
            );

            _shaders = graphicsDevice.ResourceFactory.CreateFromSpirv(vertexShader, fragmentShader);
        }

        private void CreatePipeline(Veldrid.GraphicsDevice graphicsDevice)
        {
            var vertexLayout = new Veldrid.VertexLayoutDescription(
                new Veldrid.VertexElementDescription("Position", Veldrid.VertexElementSemantic.TextureCoordinate, Veldrid.VertexElementFormat.Float2),
                new Veldrid.VertexElementDescription("TextureCoordinates", Veldrid.VertexElementSemantic.TextureCoordinate, Veldrid.VertexElementFormat.Float2)
            );

            _pipeline = graphicsDevice.ResourceFactory.CreateGraphicsPipeline(new Veldrid.GraphicsPipelineDescription
            {
                BlendState = Veldrid.BlendStateDescription.SingleDisabled,
                DepthStencilState = Veldrid.DepthStencilStateDescription.Disabled,
                RasterizerState = Veldrid.RasterizerStateDescription.CullNone,
                PrimitiveTopology = Veldrid.PrimitiveTopology.TriangleStrip,
                ResourceLayouts = new Veldrid.ResourceLayout[] { _resourceLayout },
                ShaderSet = new Veldrid.ShaderSetDescription(
                    vertexLayouts: new Veldrid.VertexLayoutDescription[] { vertexLayout },
                    shaders: _shaders
                ),
                Outputs = graphicsDevice.SwapchainFramebuffer.OutputDescription
            });
        }

        public void Draw(object objCommandList)
        {
            if (!objCommandList.GetType().IsSubclassOf(typeof(Veldrid.CommandList)))
            {
                throw new ArgumentException("Given argument is not of type CommandList");
            }

            var commandList = (Veldrid.CommandList)objCommandList;

            commandList.SetVertexBuffer(0, _vertexBuffer);
            commandList.SetIndexBuffer(_indexBuffer, Veldrid.IndexFormat.UInt16);
            commandList.SetPipeline(_pipeline);
            commandList.SetGraphicsResourceSet(0, _resourceSet);
            commandList.DrawIndexed(
                indexCount: 4,
                instanceCount: 1,
                indexStart: 0,
                vertexOffset: 0,
                instanceStart: 0
            );
        }

        private struct ImageVertex
        {
            public Vector2 Position { get; }
            public Vector2 TextureCoordinates { get; }
            public ImageVertex(Vector2 position, Vector2 textureCoordinates)
            {
                Position = position;
                TextureCoordinates = textureCoordinates;
            }
        }

        private const string VertexShader = @"
            #version 450
            layout(location = 0) in vec2 Position;
            layout(location = 1) in vec2 TextureCoordinates;

            layout(location = 0) out vec2 outTextureCoordinates;

            void main()
            {
                gl_Position = vec4(Position, 0, 1);
                outTextureCoordinates = TextureCoordinates;
            }";

        private const string FragmentShader = @"
            #version 450
            layout(set = 0, binding = 0) uniform texture2D ImageTexture;
            layout(set = 0, binding = 1) uniform sampler ImageSampler;
            layout(location = 0) in vec2 TextureCoordinates;

            layout(location = 0) out vec4 outColor;

            void main()
            {
                outColor = texture(sampler2D(ImageTexture, ImageSampler), TextureCoordinates);
            }";
    }
}
