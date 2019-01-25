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

        // TEST DATA
        private ITexture _texture = null;

        public TestingModuleManager(IEngineManager engineManager)
        {
            _engineManager = engineManager;

            // TEST DATA
            Console.WriteLine(Directory.GetCurrentDirectory());
            using (var archive = new LGPArchive())
            {
                //if (!archive.Open("../../../../Data/midi/midi.lgp"))//midi, awe, xg, ygm
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
                            var _image = (TexFile)file;
                            //                            _image.Parse();

                            _texture = _engineManager.CreateTexture(_image.GetTextureFormat(), _image.GetTextureBuffer());
                        }
                    }
                    /*
                    var file = archive.Find("aseri2.mid");
                    if (file == null)
                    {
                        Console.WriteLine("Failed to find bat.mid");
                    }
                    else
                    {
                        if (file.GetType() == typeof(MidiFile))
                        {
                            Console.WriteLine("Instance of midifile");
                            var midi = (MidiFile)file;
                            var buffer = midi.GetBuffer();
                            using (var fs = new FileStream("../../../../Extract/aseri2_midi.mid", FileMode.CreateNew))
                            {
                                var bytes = new byte[buffer.GetSize()];
                                Marshal.Copy(buffer.GetIntPtr(), bytes, 0, (int)buffer.GetSize());
                                fs.Write(bytes);
                            }
                        }
                    }
                    */
                }
            }

        }

        public void Register()
        {
            _engineManager.Register(this); ;
        }

        public void OnUpdate(long deltaTime)
        {

        }

        public void OnDraw()
        {
            if (_texture != null)
            {
                _engineManager.Draw(_texture);
            }
        }
    }
}
