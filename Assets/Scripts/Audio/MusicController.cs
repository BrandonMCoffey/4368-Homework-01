using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Audio {
    public class MusicController : MonoBehaviour {
        private AudioSource _source0;
        private AudioSource _source1;

        private bool _currentIsSource0 = true;

        private Coroutine _curSourceFadeRoutine;
        private Coroutine _newSourceFadeRoutine;

        private void Awake()
        {
            if (_source0 == null) {
                _source0 = NewSource("Music Track 1");
                _source0.loop = true;
                _source0.spatialBlend = 0;
            }
            if (_source1 == null) {
                _source1 = NewSource("Music Track 2");
                _source1.loop = true;
                _source1.spatialBlend = 0;
            }
        }

        public void SetSong(AudioClip clip, float volume = 1, float fadeTime = 1)
        {
            if (_source0.isPlaying || _source1.isPlaying) {
                CrossFade(clip, volume, fadeTime);
            } else {
                _currentIsSource0 = true;
                _source0.clip = clip;
                _source0.volume = 0;
                _source0.Play();
                _newSourceFadeRoutine = StartCoroutine(FadeSource(_source0, 0, volume, fadeTime));
            }
        }

        public void CrossFade(AudioClip clip, float maxVolume, float fadeTime)
        {
            AudioSource currentlyPlaying;
            AudioSource newSourceToPlay;
            if (_currentIsSource0) {
                currentlyPlaying = _source0;
                newSourceToPlay = _source1;
                _currentIsSource0 = false;
            } else {
                currentlyPlaying = _source1;
                newSourceToPlay = _source0;
                _currentIsSource0 = true;
            }

            newSourceToPlay.clip = clip;
            newSourceToPlay.volume = 0;
            newSourceToPlay.Play();

            if (_curSourceFadeRoutine != null) {
                StopCoroutine(_curSourceFadeRoutine);
            }
            if (_newSourceFadeRoutine != null) {
                StopCoroutine(_newSourceFadeRoutine);
            }

            _curSourceFadeRoutine = StartCoroutine(FadeSource(currentlyPlaying, currentlyPlaying.volume, 0, fadeTime));
            _newSourceFadeRoutine = StartCoroutine(FadeSource(newSourceToPlay, newSourceToPlay.volume, maxVolume, fadeTime));

            _currentIsSource0 = !_currentIsSource0;
        }

        private static IEnumerator FadeSource(AudioSource sourceToFade, float startVolume, float endVolume, float duration)
        {
            float startTime = Time.time;

            while (true) {
                float elapsed = Time.time - startTime;

                sourceToFade.volume = Mathf.Clamp01(Mathf.Lerp(startVolume, endVolume, elapsed / duration));

                if (Math.Abs(sourceToFade.volume - endVolume) < 0.01f) {
                    break;
                }

                yield return null;
            }
            sourceToFade.volume = endVolume;
        }

        private AudioSource NewSource(string objName)
        {
            GameObject newSource = new GameObject(objName);
            newSource.transform.SetParent(transform);
            return newSource.AddComponent<AudioSource>();
        }
    }
}