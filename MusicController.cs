using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmbientMusicGenerator
{
    public class MusicController : MonoBehaviour
    {
        public List<MusicPresetObject> Presets = new List<MusicPresetObject>();     // List of preset music mixes.
        public List<Sound> Sounds = new List<Sound>();                              // List of all sounds.

        public bool PlayOnStart = true;
        private bool isPlaying = false;

        public bool IsChangingTrack = false;        
        public float ChangeTrackTime = 3f;          // How long it takes to change tracks.
        private float _currChangeTrackTime = 0f;    // Time left to change track.

        public void Awake()
        {
            foreach (Sound sound in Sounds)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = sound.SoundClip;
                sound.Source = audioSource;
                sound.UpdateSource();
            }
        }

        private void Start()
        {
            // Start music automatically if enabled.
            if (PlayOnStart)
            {
                Play();
            }
        }

        private void Update()
        {
            // Updates each audio source of any changes.
            foreach (Sound sound in Sounds)
            {
                sound.UpdateSource();
            }

            if (IsChangingTrack)
            {

            }
        }

        /// <summary>
        /// Plays the music. If the music is already playing, does nothing.
        /// </summary>
        public void Play()
        {
            if (isPlaying) return;

            foreach (Sound sound in Sounds)
            {
                sound.Source.Play();
            }
            isPlaying = true;
        }

        /// <summary>
        /// Pauses the music. If the music is already paused, does nothing.
        /// </summary>
        public void Pause()
        {
            foreach (Sound sound in Sounds)
            {
                sound.Source.Pause();
            }
            isPlaying = false;
        }

        /// <summary>
        /// Toggles the music on or off.
        /// </summary>
        public void Toggle()
        {
            if (isPlaying)
            {
                Pause();
            } else
            {
                Play();
            }
        }

        /// <summary>
        /// Starts the music back to the beginning of the track and
        /// starts playing it again.
        /// </summary>
        public void Restart()
        {
            foreach (Sound sound in Sounds)
            {
                sound.Source.Play();
            }
            isPlaying = true;
        }

        /// <summary>
        /// Removes the AudioSource component associated with a specific sound.
        /// </summary>
        /// <param name="sound"></param>
        public void RemoveAudioSource(Sound sound)
        {
            try
            {
                if (sound.Source != null)
                {
                    Destroy(sound.Source);
                }
            } catch (Exception e)
            {
                Debug.LogWarning($"Error removing AudioSource: {e}");
            }
        }

        public void TestLoad()
        {
            LoadSong(Presets[0]);
        }

        /// <summary>
        /// Switches the 
        /// </summary>
        /// <param name="song"></param>
        public void LoadSong(MusicPresetObject song)
        {
            foreach (Sound newSound in song.Sounds)
            {
                bool soundFound = false;
                foreach (Sound sound in Sounds)
                {
                    if (newSound.SoundClip.Equals(sound.SoundClip))
                    {
                        Debug.Log($"Current song has sound {newSound.Name} (called {sound.Name}).");
                        soundFound = true;
                        break;
                    }
                }
                if (!soundFound)
                {
                    Debug.Log($"Current sound does not have sound {newSound.Name}.");
                }
            }
        }
    }
}