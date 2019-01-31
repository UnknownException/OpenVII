using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Audio.MidiAudio
{
    internal static class FluidSynth
    {
        [DllImport(@"FluidSynth-API.dll")]
        public static extern bool SetSoundFont(string soundFont);

        [DllImport(@"FluidSynth-API.dll")]
        public static extern bool Initialize(string defaultSoundFont);

        [DllImport(@"FluidSynth-API.dll")]
        public static extern void Destroy();

        [DllImport(@"FluidSynth-API.dll")]
        public static extern void Play(IntPtr bufferPointer, uint bufferSize);

        [DllImport(@"FluidSynth-API.dll")]
        public static extern void Stop();
    }
}
