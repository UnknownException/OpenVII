using Shared.Engine.Drawable;
using Shared.Modules;

namespace Shared.Engine
{
    public interface IEngineManager
    {
        bool CreateWindow(int width, int height, string title);
        void Run();
        void Register(IBaseModuleManager baseManager);

        ITexture CreateTexture(TextureFormat textureFormat, DataBuffer textureBuffer);
        void Draw(IDrawable drawable);
    }
}
