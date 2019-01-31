using Audio;
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

                    var file = archive.Find("pcloud.tex");
                    if (file == null)
                    {
                        Console.WriteLine("Failed to find pcloud.text");
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
            using (var archive = new LGPArchive())
            {
                if (!archive.Open("../../../../Data/midi/ygm.lgp"))
                {
                    Console.WriteLine("Failed to open ygm midi lgp");
                }
                else
                {
                    var file = archive.Find("lb2.mid");
                    if (file == null)
                    {
                        Console.WriteLine("Failed to find chu.mid");
                    }
                    else
                    {
                        if (file.GetType() == typeof(MidiFile))
                        {
                            Console.WriteLine("Instance of midi");
                            var _midiFile = (MidiFile)file;
                            var audioMgr = new AudioManager();
                            audioMgr.SetSoundFont("MuseScore_General.sf2");
                            audioMgr.PlayMidi(_midiFile.GetBuffer());
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
