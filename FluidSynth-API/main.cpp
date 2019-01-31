#include "../Extern/FluidSynthWin64/include/fluidsynth.h"
#pragma comment(lib, "../Extern/FluidSynthWin64/src/Debug/fluidsynth.lib")

fluid_settings_t* _settings = nullptr;
fluid_synth_t* _synth = nullptr;
fluid_player_t* _player = nullptr;
fluid_audio_driver_t* _adriver = nullptr;

extern "C"
{
	__declspec(dllexport) bool SetSoundFont(const char* path)
	{
		if (fluid_is_soundfont(path))
		{
			if (fluid_synth_sfload(_synth, path, 1) == -1)
			{
				return false;
			}
		}
		else
		{
			return false;
		}

		return true;
	}

	__declspec(dllexport) bool Initialize(const char* defaultSoundFont)
	{
		_settings = new_fluid_settings();
		_synth = new_fluid_synth(_settings);
		if (!_synth)
		{
			return false;
		}

		fluid_settings_setint(_settings, "synth.chorus.active", 0);
		fluid_settings_setint(_settings, "synth.reverb.active", 0);

		if (!SetSoundFont(defaultSoundFont))
		{
			return false;
		}

		_adriver = new_fluid_audio_driver(_settings, _synth);
		if (!_adriver)
		{
			return false;
		}

		_player = new_fluid_player(_synth);
		if (!_player)
		{
			return false;
		}

		return true;
	}

	__declspec(dllexport) void Destroy()
	{
		if(_player) delete_fluid_player(_player);	
		if(_adriver) delete_fluid_audio_driver(_adriver);
		if(_synth) delete_fluid_synth(_synth);
		if(_settings) delete_fluid_settings(_settings);
	}

	_declspec(dllexport) void Play(void* dataPtr, unsigned int dataSize)
	{	
		if (dataPtr)
		{
			fluid_player_add_mem(_player, dataPtr, dataSize);
			fluid_player_play(_player);
		}
	}

	_declspec(dllexport) void Stop()
	{
		fluid_player_stop(_player);
	}
}