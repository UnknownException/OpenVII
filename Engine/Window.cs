using System;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Engine
{
    internal class Window : IDisposable
    {
        protected Sdl2Window SdlWindow { get; }
        public CommandList CommandList { get; }
        public GraphicsDevice GraphicsDevice { get; }

        public Window(int width, int height, string title)
        {
            WindowCreateInfo windowCI = new WindowCreateInfo()
            {
                X = 100,
                Y = 100,
                WindowWidth = 800,
                WindowHeight = 600,
                WindowTitle = title
            };

            SdlWindow = VeldridStartup.CreateWindow(ref windowCI);

            GraphicsDevice = VeldridStartup.CreateGraphicsDevice(SdlWindow);
            CommandList = GraphicsDevice.ResourceFactory.CreateCommandList();
        }

        public void Dispose()
        {
            CommandList.Dispose();
            GraphicsDevice.Dispose();
        }

        public bool Run()
        {
            if(!SdlWindow.Exists)
            {
                return false;
            }

            SdlWindow.PumpEvents();

            return true;
        }

        public bool BeginDraw()
        {
            if (!SdlWindow.Exists)
            {
                return false;
            }

            CommandList.Begin();

            CommandList.SetFramebuffer(GraphicsDevice.SwapchainFramebuffer);
            CommandList.ClearColorTarget(0, RgbaFloat.Blue);

            return true;
        }

        public void EndDraw()
        {
            if (!SdlWindow.Exists)
            {
                return;
            }

            CommandList.End();
            GraphicsDevice.SubmitCommands(CommandList);

            GraphicsDevice.SwapBuffers();
        }
    }
}