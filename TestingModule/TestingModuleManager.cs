using FileFormats.ArchiveFormats;
using FileFormats.FileFormats;
using Shared.Engine;
using Shared.Engine.Drawable;
using Shared.Modules;
using System;
using System.IO;
using System.Runtime.InteropServices;

/*
 * Temporary development module 
 */

namespace TestingModule
{
    public class TestingModuleManager : IBaseModuleManager
    {
        private readonly IEngineManager _engineManager;

        private IImage _image = null;

        public TestingModuleManager(IEngineManager engineManager)
        {
            _engineManager = engineManager;

            Console.WriteLine(Directory.GetCurrentDirectory());
            using (var archive = new LGPArchive())
            {
                if (!archive.Open("../../../../Data/menu/menu_us.lgp"))
                {
                    Console.WriteLine("Failed to open menu_us");
                }
                else
                {
                    Console.WriteLine("Opened lgp");

                    var file = archive.Find("barre.tex");
                    if (file == null)
                    {
                        Console.WriteLine("Failed to find barre.tex");
                    }
                    else
                    {
                        if (file.GetType() == typeof(TexFile))
                        {
                            Console.WriteLine("Instance of texfile");
                            var _texFile = (TexFile)file;
                            _image = _engineManager.CreateImage(_texFile.GetTextureFormat(), _texFile.GetTextureBuffer());
                        }
                    }
                }
            }

        }

        public void Register()
        {
            _engineManager.Register(this);
        }

        public void OnUpdate(long deltaTime)
        {

        }

        public void OnDraw()
        {
            if (_image != null)
            {
                _engineManager.Draw(_image);
            }
        }
    }
}
