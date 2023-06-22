using System;
using System.Collections;
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

        public float TrackTransitionTime = 3f;          // How long it takes to change tracks.

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
                        // Start transitioning existing sound.
                        //Debug.Log($"Current song has sound {newSound.Name} (called {sound.Name}).");
                        StartCoroutine(TransitionValues(sound, newSound, TrackTransitionTime));
                        soundFound = true;
                        break;
                    }
                }
                if (!soundFound)
                {
                    // Log the new sound value and start fading it in.
                    Debug.Log($"Current song does not have sound {newSound.Name}.");
                    Sound temp = new Sound(newSound.Name, newSound.SoundClip);
                    Sounds.Add(temp);
                    StartCoroutine(TransitionValues(temp, newSound, TrackTransitionTime));
                }
            }

            // Look for sounds in the current song that need to be faded out of the new song.
            foreach (Sound sound in Sounds)
            {
                bool soundFound = false;
                foreach (Sound newSound in song.Sounds)
                {
                    if (newSound.SoundClip.Equals(sound.SoundClip))
                    {
                        soundFound = true;
                        break;
                    }
                }
                if (!soundFound)
                {
                    Debug.Log($"New song does not have sound {sound.Name}.");
                    StartCoroutine(TransitionValues(sound, Sound.zeroSound, TrackTransitionTime));
                }
            }
        }

        /// <summary>
        /// Coroutine to fade values over time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator TransitionValues(Sound curr, Sound target, float time)
        {
            float totalTime = 0;
            float startingVolume = curr.Volume;
            float startingPitch = curr.Pitch;
            float startingVolumeFadeStrength = curr.VolumeFadeStrength;
            float startingVolumeFadeFrequency = curr.VolumeFadeFrequency;
            float startingPitchFadeStrength = curr.PitchFadeStrength;
            float startingPitchFadeFrequency = curr.PitchFadeFrequency;

            for (float t = 0; t <= 1; t += Time.deltaTime / time)
            {
                curr.Volume = Mathf.Lerp(startingVolume, target.Volume, t);
                curr.Pitch = Mathf.Lerp(startingPitch, target.Pitch, t);
                curr.VolumeFadeStrength = Mathf.Lerp(startingVolumeFadeStrength, target.VolumeFadeStrength, t);
                curr.VolumeFadeFrequency = Mathf.Lerp(startingVolumeFadeFrequency, target.VolumeFadeFrequency, t);
                curr.PitchFadeStrength = Mathf.Lerp(startingPitchFadeStrength, target.PitchFadeStrength, t);
                curr.PitchFadeFrequency = Mathf.Lerp(startingPitchFadeFrequency, target.PitchFadeFrequency, t);

                totalTime += Time.deltaTime;
                yield return null;
            }
            curr.Volume = target.Volume;
            curr.Pitch = target.Pitch;
            curr.VolumeFadeStrength = target.VolumeFadeStrength;
            curr.VolumeFadeFrequency = target.VolumeFadeFrequency;
            curr.PitchFadeStrength = target.PitchFadeStrength;
            curr.PitchFadeFrequency = target.PitchFadeFrequency;
            totalTime += Time.deltaTime;

            Debug.Log($"Finished transitioning {curr.Name} in {totalTime} seconds:\n{{\tVolume {startingVolume} -> {curr.Volume} ({target.Volume})\n\tVolume {startingPitch} -> {curr.Pitch} ({target.Pitch})\n}}");
        }
    }
}