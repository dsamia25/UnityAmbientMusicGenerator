# UnityAmbientMusicGenerator
 Controls the relative volumes of different audio sources.

How to use:
- Create a GameObject and add the MusicController script component.
- In the inspector, add to Sounds array in the MusicController.
- For each Sound in the Sounds array, enter a name and add a .wav audio file.
- Adjust the volumes and pitch of each Sound in the Sounds array to create ambient music.

Presets:
- Presets can be created in the CreateAssetMenu.
- Any sound included in a preset that is not already in the MusicController will be faded in when loaded.
- Any sound in the MusicController that is not in a preset will be faded out when loaded.
- Presets can also be created in the MusicController inspector window using the "Save Preset" button.
	- The "Save Preset" button will use the input "Preset Name" and "Save Path" text fields.
	- The "Save Preset" button will save the current state of the MusicController.
	- The save path must start with "Assets".
	- The save path must be an existing file.

AudioMixer:
- Adding an AudioMixer to the MusicController will output all sounds through that AudioMixer.
- Changing Presets will change the AudioMixer in MusicController to the Preset's mixer.
- Setting OverridePresetSongMixer to true in MusicController will stop Presets from changing the AudioMixer.


