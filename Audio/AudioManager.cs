using Audio.MidiAudio;
using Shared;
using System;

namespace Audio
{
    public class AudioManager : IDisposable
    {
        private Midi _midi;

        public AudioManager()
        {
            _midi = new Midi("MuseScore_General.sf2");
        }

        public void Dispose()
        {
            _midi.Dispose();
        }

        public void SetSoundFont(string path)
        {
            if(_midi.IsPlaying())
            {
                _midi.Stop();
            }

            _midi.SetSoundFont(path);
        }

        /// <summary>
        /// Sends the midiBuffer to the external FluidSynth DLL
        /// Only one midi can be played simultaneously
        /// </summary>
        /// <param name="midiBuffer"></param>
        public void PlayMidi(DataBuffer midiBuffer)
        {
            if(_midi.IsPlaying())
            {
                _midi.Stop();
            }

            _midi.Play(midiBuffer);
        }

        public void StopMidi()
        {
            if(_midi.IsPlaying())
            {
                _midi.Stop();
            }
        }

        /// <summary>
        /// Plays a wav till it's finished, multiple can be played simultaneously
        /// </summary>
        /// <param name="wavBuffer"></param>
        public void PlayWav(byte[] wavBuffer)
        {
            throw new NotImplementedException();
        }
    }
}
