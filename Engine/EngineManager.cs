using System;
using System.Diagnostics;
using EngineModule.Drawable;
using Shared;
using Shared.Engine;
using Shared.Engine.Drawable;
using Shared.Modules;

namespace EngineModule
{
    public class EngineManager : IEngineManager
    {
        private delegate void OnUpdateDelegate(long deltaTime);
        private OnUpdateDelegate _onUpdateDelegate;

        private delegate void OnDrawDelegate();
        private OnDrawDelegate _onDrawDelegate;

        private Window _window;

        public EngineManager()
        {

        }

        public bool CreateWindow(int width, int height, string title)
        {
            _window = new Window(width, height, title);
            return true;
        }

        public void Run()
        {
            if (_window == null)
            {
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            long previousRenderTime = stopwatch.ElapsedMilliseconds;

            while(_window.Run())
            {
                var currentRenderTime = stopwatch.ElapsedMilliseconds;
                var difference = currentRenderTime - previousRenderTime;

                if (difference >= 16) // 60FPS cap
                {
                    _onUpdateDelegate(difference);

                    if (_window.BeginDraw())
                    {
                        _onDrawDelegate();
                        _window.EndDraw();
                    }

                    previousRenderTime = currentRenderTime - (difference - 16);
                }
            }

            stopwatch.Stop();

            _window.Dispose();
            _window = null;
        }

        public void Register(IBaseModuleManager baseManager)
        {
            _onUpdateDelegate += baseManager.OnUpdate;
            _onDrawDelegate += baseManager.OnDraw;
        }

        public ITexture CreateTexture(TextureFormat textureFormat, DataBuffer textureBuffer)
        {
            if (_window == null)
            {
                return null;
            }


            return new Texture(_window.GraphicsDevice, textureFormat, textureBuffer);
        }

        public void Draw(IDrawable drawable)
        {
            if(_window == null)
            {
                return;
            }

            drawable.Draw(_window.CommandList);
        }
    }
}
