using UnityEngine;

namespace Assets.Scripts.Utility {
    public static class AudioHelper {
        public static AudioSource PlayClip2D(AudioClip clip, float volume = 1, bool destroy = true)
        {
            // create
            GameObject audioObject = new GameObject("Audio2D");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            // configure
            audioSource.clip = clip;
            audioSource.volume = volume;
            // activate
            audioSource.Play();
            if (destroy) {
                Object.Destroy(audioSource, clip.length);
            }
            // return (in case other things need it)
            return audioSource;
        }
    }
}