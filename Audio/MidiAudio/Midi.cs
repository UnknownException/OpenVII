using System;
using System.Collections.Generic;
using System.Text;
using Shared;

namespace Audio.MidiAudio
{
    internal class Midi : IDisposable
    {
        private DataBuffer _midiBuffer; // Keep databuffer to prevent GC
        
        private enum MidiState
        {
            None,
            Playing,
            Finished,
        }

        private MidiState _state;

        public Midi(string defaultSoundFontPath)
        {
            _state = MidiState.None;

            if(!FluidSynth.Initialize(defaultSoundFontPath))
            {
                throw new Exception("Midi initializing failed");
            }
        }

        public void Dispose()
        {
            FluidSynth.Destroy();
        }

        public void SetSoundFont(string path)
        {
            if (_state == MidiState.Playing)
            {
                throw new InvalidOperationException("Midi must be stopped before assigning a new soundfont");
            }

            FluidSynth.SetSoundFont(path);
        }

        /* FluidSynth-api defaults to looping */
        public bool IsPlaying()
        {
            return _state == MidiState.Playing;
        }

        public void Stop()
        {
            FluidSynth.Stop();

            _midiBuffer = null; // Clear databuffer for GC
            _state = MidiState.Finished;
        }

        public void Play(DataBuffer midiBuffer)
        {
            if(_state == MidiState.Playing)
            {
                throw new InvalidOperationException("Midi must be stopped before playing assigning a new buffer");
            }

            _midiBuffer = midiBuffer;
            _state = MidiState.Playing;

            FluidSynth.Play(midiBuffer.GetIntPtr(), midiBuffer.GetSize());
        }
    }
}
